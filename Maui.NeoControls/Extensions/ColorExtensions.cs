using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Maui.NeoControls.Extensions
{
    internal static class ColorExtensions
    {
        private const float DEFAULT_SIGMA = -6f;

        internal static SKImageFilter ToSKDropShadow(this Color shadowColor, float distance)
        {
            return SKImageFilter.CreateDropShadowOnly(
                    distance,
                    distance,
                    DEFAULT_SIGMA,
                    DEFAULT_SIGMA,
                    shadowColor.ToSKColor());
        }
    }
}
