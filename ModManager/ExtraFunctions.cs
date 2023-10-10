using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModManager
{
    public class Mod
    {
        public string Name { get; set; }

        public string Slug { get; set; }
        public string Thumbnail_Url { get; set; }

        public string User_Name { get; set; }

        public string Latest_Version { get; set; }

        public ICommand DownloadCommand => new Command(OnDownloadClicked);

        private void OnDownloadClicked()
        {
            Debug.WriteLine($"Clicked download for mod: {Name}");
        }
    }


    public class ApiResponse
    {
        public List<Mod> Mods { get; set; }
        // ... other properties as needed
    }

    public class GenericFunctions
    {
        public static string gameDir;
        public static string modDir;

        public static void StartUpFunctions()
        {

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

    }

}
