using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModManager
{
    public class Mod : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public string Slug { get; set; }
        public string Thumbnail_Url { get; set; }

        public string User_Name { get; set; }

        public string User_Slug { get; set; }

        public string Latest_Version { get; set; }

        public string Mod_Id { get; set; }

        // Download Progress
        private double _downloadProgress = 0;

        public string DownloadPercentageText
        {
            get
            {
                if (_downloadProgress >= 1)
                {
                    return "Complete";
                }
                return $"{(_downloadProgress * 100):0}%";
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double DownloadProgress
        {
            get { return _downloadProgress; }
            set
            {
                _downloadProgress = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DownloadPercentageText)); // Notify change for the percentage text.
            }
        }

        private bool _isDownloading = false;
        public bool IsDownloading
        {
            get { return _isDownloading; }
            set
            {
                _isDownloading = value;
                OnPropertyChanged();
            }
        }


        // FOR DOWNLOADING
        public ICommand DownloadCommand => new Command(OnDownloadClicked);

        private async void OnDownloadClicked()
        {
            string apiPath = $"https://api.sotf-mods.com/api/mods/{Mod_Id}/download/{Latest_Version}";
            Debug.WriteLine($"Clicked download for mod: {Name}");
            Debug.WriteLine($"API PATH: {apiPath}");

            var destinationPath = Settings.savedModDir;

            if (Directory.Exists(destinationPath) == false || Settings.savedModDir == null)
            {

                ShowWarning("Mod Directory Not Found, Please Goto Settings");
                Debug.WriteLine("Mod Directory Not Found, Please Goto Settings");
                return;
            }

            // 1. Check if the mod folder or DLL exists
            if (Directory.Exists(Path.Combine(destinationPath, Name)) || File.Exists(Path.Combine(destinationPath, $"{Name}.dll")))
            {
                // 3. Display a warning
                ShowWarning($"The mod '{Name}' or its DLL already exists. Download cancelled.");
                DownloadProgress = 0;
                IsDownloading = false;
                return; // 2. Cancel the download if they exist
            }

            try
            {
                IsDownloading = true;

                using (var httpClient = new HttpClient())
                {
                    var fileBytes = await httpClient.GetByteArrayAsync(apiPath);
                    DownloadProgress = 0.33;

                    // 2. Save the file to a temporary directory
                    var tempZipFilePath = Path.GetTempFileName();
                    await File.WriteAllBytesAsync(tempZipFilePath, fileBytes);
                    DownloadProgress = 0.37;

                    // Create a temp directory to extract to
                    var tempExtractPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(tempExtractPath);
                    DownloadProgress = 0.39;

                    // 3. Unzip the file
                    ZipFile.ExtractToDirectory(tempZipFilePath, tempExtractPath);
                    DownloadProgress = 0.66;

                    // If files are inside a extre Mods folder
                    var directories = Directory.GetDirectories(tempExtractPath);
                    if (directories.Length == 1 && Path.GetFileName(directories[0]) == "Mods")
                    {
                        tempExtractPath = directories[0];
                    }


                    // 4. Delete the zipped file
                    File.Delete(tempZipFilePath);
                    DownloadProgress = 0.70;

                    // 5. Move the contents of the unzipped folder to desired location
                    foreach (var dirPath in Directory.GetDirectories(tempExtractPath, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dirPath.Replace(tempExtractPath, destinationPath));
                        
                    }
                    DownloadProgress = 0.80;
                    foreach (var filePath in Directory.GetFiles(tempExtractPath, "*.*", SearchOption.AllDirectories))
                    {
                        File.Move(filePath, filePath.Replace(tempExtractPath, destinationPath), true);
                    }
                    DownloadProgress = 0.90;
                    Directory.Delete(tempExtractPath, true); // Clean up temp extraction directory
                    DownloadProgress = 1;

                }
            } catch (Exception ex)
            {
                Debug.WriteLine($"Error during download and installation: {ex.Message}");
                //await DisplayAlert("Error", $"Error during download and installation: {ex.Message}", "OK");
                DownloadProgress = 0;
                ShowWarning("Mod Failed To Download");
                IsDownloading = false;
            }
            

        }


        public event Action<string> ShowWarningRequested;

        private void ShowWarning(string message)
        {
            ShowWarningRequested?.Invoke(message);
        }

    }


    public class ApiResponse
    {
        public List<Mod> Mods { get; set; }
        // ... other properties as needed
    }

    public class GenericFunctions
    {
        public static void StartUpFunctions()
        {
            CheckAndSetGamePath();
            CheckAndSetModPath();
        }

        public static string GetGameDir()
        {
            if (GameDirectoryExists())
            {
                return @"C:\Program Files (x86)\Steam\steamapps\common\Sons Of The Forest";
            }
            else
            {
                return null;
            }
        }

        public static string GetModDir()
        {
            if (ModDirectoryExists())
            {
                return @"C:\Program Files (x86)\Steam\steamapps\common\Sons Of The Forest\Mods";
            }
            else
            {
                return null;
            }
        }

        public static bool GameDirectoryExists()
        {
            string directoryPath = @"C:\Program Files (x86)\Steam\steamapps\common\Sons Of The Forest";

            if (Directory.Exists(directoryPath))
            {
                Debug.WriteLine("GameDirectory directory exists.");
                return true;
            }
            else
            {
                Debug.WriteLine("GameDirectory directory does not exist.");
                return false;
            }
        }

        public static bool ModDirectoryExists()
        {
            string directoryPath = @"C:\Program Files (x86)\Steam\steamapps\common\Sons Of The Forest\Mods";

            if (Directory.Exists(directoryPath))
            {
                Debug.WriteLine("Mod directory exists.");
                return true;
            }
            else
            {
                Debug.WriteLine("Mod directory does not exist.");
                return false;
            }
        }

        private static void CheckAndSetGamePath()
        {
            if (string.IsNullOrEmpty(Settings.savedGameDir))
            {
                if (GenericFunctions.GetGameDir() != null)
                {
                    Settings.savedGameDir = GenericFunctions.GetGameDir();
                    Debug.WriteLine("savedGameDir was null, but GameDir Standard location was found, and are now set");
                }
                else
                {
                    Debug.WriteLine("Game Directory Not Found, Please Enter");
                    Application.Current.MainPage.DisplayAlert("WARNING", "Game Directory Not Found, Please Go To Settings", "OK");
                }
            }
            else
            {
                Debug.WriteLine($"Game Directory Found: {Settings.savedGameDir}");
            }
        }

        private static void CheckAndSetModPath()
        {
            if (string.IsNullOrEmpty(Settings.savedModDir))
            {
                if (GenericFunctions.GetModDir() != null)
                {
                    Settings.savedModDir = GenericFunctions.GetModDir();
                    Debug.WriteLine("savedModDir was null, but ModDir Standard location was found, and are now set");
                }
                else
                {
                    Debug.WriteLine("Mod Directory Not Found, Are You Sure RedLoader Is Installed?");
                    Application.Current.MainPage.DisplayAlert("WARNING", "Mod Directory Not Found, Please Go To Settings", "OK");
                }
            }
            else
            {
                Debug.WriteLine($"Mod Directory Found: {Settings.savedModDir}");
            }
        }

    }

}
