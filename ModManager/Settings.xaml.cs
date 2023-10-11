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

    public string savedGameDir
    {
        get { return Preferences.Get(nameof(savedGameDir), null); }
        set { Preferences.Set(nameof(savedGameDir), value); }
    }

    public string savedModDir
    {
        get { return Preferences.Get(nameof(savedModDir), null); }
        set { Preferences.Set(nameof(savedModDir), value); }
    }

    private void CheckAndSetGamePath()
    {
        //if (GenericFunctions.GetGameDir() == null)
        //{
        //    GameDirectoryEntry.Text = "Game Directory Not Found, Please Enter";
        //    DisplayAlert("Alert", "Game Directory Not Found, Please Enter", "OK");
        //    Debug.WriteLine("Game Directory Not Found, Please Enter");
        //} else
        //{
        //    GameDirectoryEntry.Text = GenericFunctions.GetGameDir();
        //    Debug.WriteLine($"Game Directory Found: {GenericFunctions.GetGameDir()}");
        //}

        if (string.IsNullOrEmpty(savedGameDir))
        {
            GameDirectoryEntry.Text = "Game Directory Not Found, Please Enter";
            DisplayAlert("Alert", "Game Directory Not Found, Please Enter", "OK");
            Debug.WriteLine("Game Directory Not Found, Please Enter");
        }
        else
        {
            GameDirectoryEntry.Text = savedGameDir;
            Debug.WriteLine($"Game Directory Found: {savedGameDir}");
        }
    }

    private void CheckAndSetModPath()
    {
        //if (GenericFunctions.GetModDir() == null)
        //{
        //    ModDirectoryEntry.Text = "Mod Directory Not Found, Are You Sure RedLoader Is Installed?";
        //    DisplayAlert("Alert", "Mod Directory Not Found, Are You Sure RedLoader Is Installed?", "OK");
        //    Debug.WriteLine("Mod Directory Not Found, Are You Sure RedLoader Is Installed?");
        //}
        //else
        //{
        //    ModDirectoryEntry.Text = GenericFunctions.GetModDir();
        //    Debug.WriteLine($"Game Directory Found: {GenericFunctions.GetModDir()}");
        //}
        if (string.IsNullOrEmpty(savedModDir))
        {
            ModDirectoryEntry.Text = "Mod Directory Not Found, Are You Sure RedLoader Is Installed?";
            DisplayAlert("Alert", "Mod Directory Not Found, Are You Sure RedLoader Is Installed?", "OK");
            Debug.WriteLine("Mod Directory Not Found, Are You Sure RedLoader Is Installed?");
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
        GenericFunctions.gameDir = GameDirectoryEntry.Text;
        savedGameDir = GameDirectoryEntry.Text;
        DisplayAlert("Note", "Game Directory Saved", "OK");

    }

    private void OnModDirSaveButtonButtonClicked(object sender, EventArgs e)
    {
        // Your code to execute when the button is clicked
        Debug.WriteLine("Save button clicked!");
        GenericFunctions.modDir = ModDirectoryEntry.Text;
        DisplayAlert("Note", "Mod Directory Saved", "OK");

    }

    //public async Task LoadSaveSettings()
    //{
    //    if (settingsService == null)
    //    {
    //        Debug.WriteLine("settingsService is null!");
    //        return;
    //    }

    //    savedGameDir = await settingsService.Get<string>(nameof(savedGameDir), null);
    //    savedModDir = await settingsService.Get<string>(nameof(savedModDir), null);

    //    Debug.WriteLine($"LoadSaveSettings SaveGameDir: {savedGameDir}");
    //    Debug.WriteLine($"LoadSaveSettings SaveModDir: {savedModDir}");
    //}

    //public async Task SaveSettings()
    //{
    //    await settingsService.Save(nameof(savedGameDir), savedGameDir);
    //    await settingsService.Save(nameof(savedModDir), savedModDir);
    //    await DisplayAlert("Saved!", "Settings has been saved!", "OK");
    //}
}