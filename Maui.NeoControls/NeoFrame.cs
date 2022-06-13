using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Maui.NeoControls;

public class NeoFrame : NeoRoundedView
{
    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
       propertyName: nameof(BorderColor),
       returnType: typeof(Color),
       declaringType: typeof(NeoFrame),
       defaultValue: Colors.Transparent);

    public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(
        propertyName: nameof(BorderWidth),
        returnType: typeof(double),
        declaringType: typeof(NeoView),
        defaultValue: 1.0);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public double BorderWidth
    {
        get => (double)GetValue(BorderWidthProperty);
        set => SetValue(BorderWidthProperty, value);
    }

    protected override void DrawControl(RenderContext renderContext)
    {
        var drawPadding = ShadowDrawMode == ShadowDrawMode.InnerOnly ?
            0 : Convert.ToSingle(ShadowBlur * 2);

        var diameter = drawPadding * 2;
        var retangleWidth = renderContext.Info.Width - diameter;
        var retangleHeight = renderContext.Info.Height - diameter;

        using var path = CreatePath(retangleWidth, retangleHeight, drawPadding);

        renderContext.Paint.ImageFilter = null;
        if (DrawMode == DrawMode.Flat)
            renderContext.Paint.MaskFilter = null;

        renderContext.Canvas.DrawPath(path, renderContext.Paint);

        if (BorderColor != Colors.Transparent)
            DrawBorder(renderContext, path);
    }

    protected virtual void DrawBorder(RenderContext renderContext, SKPath path)
    {
        renderContext.Paint.Style = SKPaintStyle.Stroke;
        renderContext.Paint.Color = BorderColor.ToSKColor();
        renderContext.Paint.StrokeWidth = Convert.ToSingle(BorderWidth);
        renderContext.Canvas.DrawPath(path, renderContext.Paint);
    }
}
