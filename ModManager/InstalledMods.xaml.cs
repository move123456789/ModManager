using System.Diagnostics;
using System.Text.Json;

namespace ModManager;

public partial class InstalledMods : ContentPage
{
	public InstalledMods()
	{
		InitializeComponent();
        doTaskFromNotAsync();
        RefreshButton.Clicked += RefreshModsButtonClick;

    }

    private void RefreshModsButtonClick(object sender, EventArgs e)
    {
        ModDetailsStack.Children.Clear();
        doTaskFromNotAsync();
        Debug.WriteLine("RefreshModsButtonClick");
    }

    public class Manifest
    {
        public string Author { get; set; }
        public string Version { get; set; }
    }

    private async void doTaskFromNotAsync()
    {
        await LoadFoldersAndManifests();
    }

    private async Task LoadFoldersAndManifests()
    {
        string directoryPath = Settings.savedModDir;

        if (Directory.Exists(directoryPath) && Settings.savedModDir != null)
        {
            var folders = Directory.GetDirectories(directoryPath);

            foreach (var folder in folders)
            {
                string manifestPath = Path.Combine(folder, "manifest.json");

                if (File.Exists(manifestPath))
                {
                    string json = await File.ReadAllTextAsync(manifestPath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Manifest manifest = JsonSerializer.Deserialize<Manifest>(json, options);

                    var folderName = Path.GetFileName(folder);
                    var dllExists = File.Exists(Path.Combine(directoryPath, folderName + ".dll"));

                    var dllOldPath = Path.Combine(directoryPath, folderName + ".old");
                    var dllPath = Path.Combine(directoryPath, folderName + ".dll");

                    // Create mod details
                    var modDetails = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        
                        Children =
                    {
                        new Label { Text = $"{folderName}", WidthRequest = 150, VerticalOptions = LayoutOptions.Center },
                        new Label { Text = $"Author: {manifest.Author}", WidthRequest = 150, VerticalOptions = LayoutOptions.Center },
                        new Label { Text = $"Version: {manifest.Version}", WidthRequest = 100, VerticalOptions = LayoutOptions.Center }
                    }
                    };

                    // Create buttons
                    var modButtons = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Margin = new Thickness(0, 10, 10, 10),
                        Children =
                    {
                        new Button { Text = "Enable", IsVisible = !dllExists, Command = new Command(() => EnableMod(dllOldPath)), BackgroundColor = Color.FromArgb("#2b2d42"), TextColor = Color.FromArgb("#edf2f4")  },
                        new Button { Text = "Disable", IsVisible = dllExists, Command = new Command(() => DisableMod(dllPath)), BackgroundColor = Color.FromArgb("#2b2d42"), TextColor = Color.FromArgb("#edf2f4") },
                        new Button { Text = "Uninstall", Command = new Command(() => UninstallMod(directoryPath, folderName)), BackgroundColor = Color.FromArgb("#2b2d42"), TextColor = Color.FromArgb("#edf2f4") }
                    }
                    };

                    // Create mod frame
                    var modFrame = new Frame
                    {
                        Content = new StackLayout
                        {
                            Children = { modDetails, modButtons }
                        },
                        CornerRadius = 10,
                        Padding = 10,
                        Margin = new Thickness(10, 5),
                        VerticalOptions = LayoutOptions.Start,
                        WidthRequest = 800,
                        BackgroundColor = Color.FromArgb("#457b9d"),
                    };

                    ModDetailsStack.Children.Add(modFrame);
                }
            }
        }
        else
        {
            await DisplayAlert("Alert", "Mod Directory Not Found, Please Goto Settings", "OK");
            Debug.WriteLine("Mod Directory Not Found, Please Goto Settings");
        }
    }

    private void EnableMod(string dllPath)
    {
        string newFilePath = Path.ChangeExtension(dllPath, ".dll");

        if (File.Exists(dllPath))
        {
            File.Move(dllPath, newFilePath);
            Debug.WriteLine($"File renamed from {dllPath} to {newFilePath}");
        }
        ModDetailsStack.Children.Clear();
        doTaskFromNotAsync();
        Debug.WriteLine("My Mods Refreshed");
    }

    private void DisableMod(string dllPath)
    {
        string newFilePath = Path.ChangeExtension(dllPath, ".old");
        if (File.Exists(dllPath))
        {
            File.Move(dllPath, newFilePath);
            Debug.WriteLine($"File renamed from {dllPath} to {newFilePath}");
        }
        ModDetailsStack.Children.Clear();
        doTaskFromNotAsync();
        Debug.WriteLine("My Mods Refreshed");
    }

    private void UninstallMod(string directoryPath, string folderName)
    {
        var dllOldPath = Path.Combine(directoryPath, folderName + ".old");
        var dllPath = Path.Combine(directoryPath, folderName + ".dll");
        var pdbPath = Path.Combine(directoryPath, folderName + ".pdb");
        var folderPath = Path.Combine(directoryPath, folderName);

        if (File.Exists(dllOldPath))
        {
            File.Delete(dllOldPath);
        }
        if (File.Exists(dllPath))
        {
            File.Delete(dllPath);
        }
        if (File.Exists(pdbPath))
        {
            File.Delete(pdbPath);
        }
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
        Debug.WriteLine("Deleted Mod");


        ModDetailsStack.Children.Clear();
        doTaskFromNotAsync();
        Debug.WriteLine("My Mods Refreshed");
    }

    private void OnButtonPressed(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            button.Scale = 0.8;
        }
    }

    private void OnButtonReleased(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            button.Scale = 1;
        }
    }

}