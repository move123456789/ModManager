using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace ModManager
{
    public partial class MainPage : ContentPage
    {
        private List<Mod> _originalMods = new List<Mod>();

        private ObservableCollection<Mod> _mods = new ObservableCollection<Mod>();
        public ObservableCollection<Mod> Mods
        {
            get { return _mods; }
            set { _mods = value; OnPropertyChanged(); }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadDataAsync(); // Call the asynchronous method here.
            GenericFunctions.StartUpFunctions();
        }

        private void OnDownloadClicked(object sender, EventArgs e)
        {
            var imageUrl = (sender as Button).CommandParameter.ToString();

            // Handle the download action using the imageUrl or other necessary information
        }


        private async void LoadDataAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            Debug.WriteLine("In LoadData()");
            try
            {
                using (var client = new HttpClient())
                {
                    var jsonResponse = await client.GetStringAsync("https://api.sotf-mods.com/api/mods?page=1&limit=1000&orderby=newest");
                    Debug.WriteLine(jsonResponse);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse, options);

                    if (apiResponse?.Mods != null)
                    {
                        Mods.Clear();
                        _originalMods.Clear();
                        foreach (var mod in apiResponse.Mods)
                        {
                            mod.ShowWarningRequested += DisplayWarning;
                            //Mods.Add(mod);
                            _originalMods.Add(mod);
                        }
                        Mods = new ObservableCollection<Mod>(_originalMods);
                    }
                    else
                    {
                        Debug.WriteLine("API response is null or Mods property is null.");
                    }

                    Debug.WriteLine($"Total mods loaded: {Mods.Count}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                // Handle exceptions appropriately, e.g., show a user-friendly error message
            }
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

        private void DisplayWarning(string message)
        {
            // Use your platform-specific code to show the warning.
            // For instance, in MAUI:
            this.DisplayAlert("Warning", message, "OK");
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Reset to the original mod list
                Mods = new ObservableCollection<Mod>(_originalMods);
            }
            else
            {
                // Filter the mod list
                var filteredMods = _originalMods.Where(mod => mod.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                Mods = new ObservableCollection<Mod>(filteredMods);
            }
        }



    }
}