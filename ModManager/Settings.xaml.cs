using System.Diagnostics;
using Windows.Gaming.Input;
using Windows.Gaming.UI;

namespace ModManager;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
        GameDirSaveButton.Clicked += OnGameDirSaveButtonButtonClicked;
        ModDirSaveButton.Clicked += OnModDirSaveButtonButtonClicked;
        CheckAndSetGamePath();
        CheckAndSetModPath();
    }

    private void CheckAndSetGamePath()
    {
        if (GenericFunctions.GetGameDir() == null)
        {
            GameDirectoryEntry.Text = "Game Directory Not Found, Please Enter";
            DisplayAlert("Alert", "Game Directory Not Found, Please Enter", "OK");
            Debug.WriteLine("Game Directory Not Found, Please Enter");
        } else
        {
            GameDirectoryEntry.Text = GenericFunctions.GetGameDir();
            Debug.WriteLine($"Game Directory Found: {GenericFunctions.GetGameDir()}");
        }
    }

    private void CheckAndSetModPath()
    {
        if (GenericFunctions.GetModDir() == null)
        {
            ModDirectoryEntry.Text = "Mod Directory Not Found, Are You Sure RedLoader Is Installed?";
            DisplayAlert("Alert", "Mod Directory Not Found, Are You Sure RedLoader Is Installed?", "OK");
            Debug.WriteLine("Mod Directory Not Found, Are You Sure RedLoader Is Installed?");
        }
        else
        {
            ModDirectoryEntry.Text = GenericFunctions.GetModDir();
            Debug.WriteLine($"Game Directory Found: {GenericFunctions.GetModDir()}");
        }
    }

    private void OnGameDirSaveButtonButtonClicked(object sender, EventArgs e)
    {
        // Your code to execute when the button is clicked
        Debug.WriteLine("Save button clicked!");
        GenericFunctions.gameDir = GameDirectoryEntry.Text;
        DisplayAlert("Note", "Game Directory Saved", "OK");

    }

    private void OnModDirSaveButtonButtonClicked(object sender, EventArgs e)
    {
        // Your code to execute when the button is clicked
        Debug.WriteLine("Save button clicked!");
        GenericFunctions.modDir = ModDirectoryEntry.Text;
        DisplayAlert("Note", "Mod Directory Saved", "OK");

    }
}