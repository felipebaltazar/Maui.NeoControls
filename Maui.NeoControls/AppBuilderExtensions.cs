using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Maui.NeoControls
{
    public static class AppBuilderExtensions
    {
        public static MauiAppBuilder UseNeoControls(this MauiAppBuilder builder)
        {
            builder.UseSkiaSharp();
            return builder;
        }
    }
}
