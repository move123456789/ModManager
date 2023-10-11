namespace ModManager;

public partial class TestPage : ContentPage
{
	public TestPage()
	{
		InitializeComponent();
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