using Maui.NeoControls.Extensions;
using SkiaSharp;
using System.Globalization;

namespace Maui.NeoControls;

public abstract class NeoRoundedView : NeoView
{
    private const int DEFAULT_CORNER_RADIUS = 3;

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        propertyName: nameof(CornerRadius),
        returnType: typeof(CornerRadius),
        declaringType: typeof(NeoRoundedView),
        defaultValue: new CornerRadius(DEFAULT_CORNER_RADIUS),
        propertyChanged: OnVisualPropertyChanged);

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    protected override SKPath CreatePath(float retangleWidth, float retangleHeight, float drawPadding)
    {
        var realWidth = Width > 0 ? Width : WidthRequest;
        var realHeight = Height > 0 ? Height : HeightRequest;

        var scaleX = retangleWidth / realWidth;
        var scaleY = retangleHeight / realHeight;

        var scale = Math.Min(scaleX, scaleY);

        var path = new SKPath();
        var fTopLeftRadius = Convert.ToSingle(CornerRadius.TopLeft * scale, CultureInfo.InvariantCulture);
        var fTopRightRadius = Convert.ToSingle(CornerRadius.TopRight * scale, CultureInfo.InvariantCulture);
        var fBottomLeftRadius = Convert.ToSingle(CornerRadius.BottomLeft * scale, CultureInfo.InvariantCulture);
        var fBottomRightRadius = Convert.ToSingle(CornerRadius.BottomRight * scale, CultureInfo.InvariantCulture);

        var startX = fTopLeftRadius + drawPadding;
        var startY = drawPadding;

        path.MoveTo(startX, startY);

        path.LineTo(retangleWidth - fTopRightRadius + drawPadding, startY);
        path.ArcTo(fTopRightRadius,
            new SKPoint(retangleWidth + drawPadding, fTopRightRadius + drawPadding));

        path.LineTo(retangleWidth + drawPadding, retangleHeight - fBottomRightRadius + drawPadding);
        path.ArcTo(fBottomRightRadius,
             new SKPoint(retangleWidth - fBottomRightRadius + drawPadding, retangleHeight + drawPadding));

        path.LineTo(fBottomLeftRadius + drawPadding, retangleHeight + drawPadding);
        path.ArcTo(fBottomLeftRadius,
            new SKPoint(drawPadding, retangleHeight - fBottomLeftRadius + drawPadding));

        path.LineTo(drawPadding, fTopLeftRadius + drawPadding);
        path.ArcTo(fTopLeftRadius, new SKPoint(startX, startY));

        path.Close();

        return path;
    }
}
