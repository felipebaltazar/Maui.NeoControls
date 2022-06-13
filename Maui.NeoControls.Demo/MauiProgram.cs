namespace Maui.NeoControls.Demo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseNeoControls()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("RobotoCondensed-Bold.ttf", "RobotoCondensedBold");
                fonts.AddFont("RobotoCondensed-Light.ttf", "RobotoCondensedRegular");
                fonts.AddFont("RobotoCondensed-Regular.ttf", "RobotoCondensedRegular");
            });

		return builder.Build();
	}
}
