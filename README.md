# Maui.NeoControls

Controls for Maui based on neumorphism tendency

 [![NuGet](https://img.shields.io/nuget/v/NeoControls.Maui.svg)](https://www.nuget.org/packages/NeoControls.Maui/)
 
 [![Build and publish packages](https://github.com/felipebaltazar/Maui.NeoControls/actions/workflows/PackageCI.yml/badge.svg)](https://github.com/felipebaltazar/Maui.NeoControls/actions/workflows/PackageCI.yml)

## Examples

![neocontrols maui](https://user-images.githubusercontent.com/19656249/173261798-dee8093a-1dad-4e79-a35d-139bfac66fd5.gif)


## Getting started

- Install the NeoControls.Maui package

 ```
 Install-Package NeoControls.Maui -Version 0.0.1-pre
 ```

- Add UseNeoControls declaration to your MauiAppBuilder

```csharp
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
    });

		return builder.Build();
	}
}
```


- Use the controls

```xml
    <NeoButton Elevation=".25"
               CornerRadius="70,20,20,20"
               BackgroundColor="#e3edf7"/>
```

- You can also insert any view inside the neo controls

```xml
        <NeoButton BackgroundColor="#e3edf7">
            
            <StackLayout Orientation="Vertical">
                <Image Source="MyImage.png "/>
                <Label Text="My Button Label"/>
            </StackLayout>
            
        </NeoButton>
```

- Background with gradient

```xml
    <NeoButton>
        <NeoButton.BackgroundGradient>
            <LinearGradient Angle="45">
                <NeoGradientStop Color="Red" Offset="0" />
                <NeoGradientStop Color="Yellow" Offset="1" />
            </LinearGradient>
        </NeoButton.BackgroundGradient>

        <StackLayout Orientation="Vertical">
            <Image Source="MyImage.png "/>
            <Label Text="My Button Label"/>
        </StackLayout>
    </NeoButton>
```

## Property reference

| Property            | What it does                                                          | Extra info                                                                 |
| ------------------- | --------------------------------------------------------------------- | -------------------------------------------------------------------------- |
| `CornerRadius`      | A `CornerRadius` object representing each individual corner's radius. | Uses the `CornerRadius` struct allowing you to specify individual corners. |
| `Elevation`         | Set this value to chenge element depth effect.                        |                                                                            |
| `InnerView`         | View that will be shown inside the neo control.                       |                                                                            |
| `ShadowBlur`        | Set this value to change shadow blur effect.                          |                                                                            |
| `ShadowDistance`    | Set this value to change shadow distance relative from control.       |                                                                            |
| `DarkShadowColor`   | The Dark color that will be applied on draw the dark shadow.          | This will be applied with `Elevation` property, as Alpha parameter.        |
| `LightShadowColor`  | The White color that will be applied on draw the light shadow.        |                                                                            |
| `BackgroundGradient`| Draw a gradient on background's control                               | When value != null, backgroundColor Property will be ignored               |



