namespace Maui.NeoControls.Demo;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await progressBar.AnimateProgress(.4f, 2500, Easing.SinIn);
    }

    private async void NeoButton_Clicked(object sender, EventArgs e)
    {
        await progressBar.AnimateProgress(.4f, 2500, Easing.SinIn);
    }
}

