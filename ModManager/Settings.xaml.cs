using System.Diagnostics;


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

    internal static string savedGameDir
    {
        get { return Preferences.Get(nameof(savedGameDir), null); }
        set { Preferences.Set(nameof(savedGameDir), value); }
    }

    internal static string savedModDir
    {
        get { return Preferences.Get(nameof(savedModDir), null); }
        set { Preferences.Set(nameof(savedModDir), value); }
    }

    private void CheckAndSetGamePath()
    {
        if (string.IsNullOrEmpty(savedGameDir))
        {
            if (GenericFunctions.GetGameDir() != null)
            {
                GameDirectoryEntry.Text = GenericFunctions.GetGameDir();
                savedGameDir = GenericFunctions.GetGameDir();
                Debug.WriteLine("savedGameDir was null, but GameDir Standard location was found, and are now set");
            }
            else
            {
                GameDirectoryEntry.Text = "Game Directory Not Found, Please Enter";
                DisplayAlert("Alert", "Game Directory Not Found, Please Enter", "OK");
                Debug.WriteLine("Game Directory Not Found, Please Enter");
            }
        }
        else
        {
            GameDirectoryEntry.Text = savedGameDir;
            Debug.WriteLine($"Game Directory Found: {savedGameDir}");
        }
    }

    private void CheckAndSetModPath()
    {
        if (string.IsNullOrEmpty(savedModDir))
        {
            if (GenericFunctions.GetModDir() != null)
            {
                ModDirectoryEntry.Text = GenericFunctions.GetModDir();
                savedModDir = GenericFunctions.GetModDir();
                Debug.WriteLine("savedModDir was null, but ModDir Standard location was found, and are now set");
            }
            else
            {
                ModDirectoryEntry.Text = "Mod Directory Not Found, Are You Sure RedLoader Is Installed?";
                DisplayAlert("Alert", "Mod Directory Not Found, Are You Sure RedLoader Is Installed?", "OK");
                Debug.WriteLine("Mod Directory Not Found, Are You Sure RedLoader Is Installed?");
            }
        }
        else
        {
            ModDirectoryEntry.Text = savedModDir;
            Debug.WriteLine($"Mod Directory Found: {savedModDir}");
        }
    }

    private void OnGameDirSaveButtonButtonClicked(object sender, EventArgs e)
    {
        // Your code to execute when the button is clicked
        Debug.WriteLine("Save button clicked!");
        savedGameDir = GameDirectoryEntry.Text;
        DisplayAlert("Note", "Game Directory Saved", "OK");

    }

    private void OnModDirSaveButtonButtonClicked(object sender, EventArgs e)
    {
        // Your code to execute when the button is clicked
        Debug.WriteLine("Save button clicked!");
        savedModDir = GameDirectoryEntry.Text;
        DisplayAlert("Note", "Mod Directory Saved", "OK");

    }
}