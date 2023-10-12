using System.Diagnostics;

namespace ModManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void OnButtonPressed(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.BackgroundColor = Color.FromArgb("#91171F");
            }
        }

        private void OnButtonReleased(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.BackgroundColor = Color.FromArgb("#BA1F33");
            }
            

            // STARTS SONS OF THE FOREST VIA CMD VIA STEAM
            string steamUrl = $"steam://run/1326470";

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C start {steamUrl}",
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error launching the game via Steam: {ex.Message}");
            }
        }
    }
}