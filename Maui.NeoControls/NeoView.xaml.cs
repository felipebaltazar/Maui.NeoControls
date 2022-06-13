using Maui.NeoControls.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Maui.NeoControls;

[ContentProperty(nameof(InnerView))]
public abstract partial class NeoView : ContentView
{
    public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(
        propertyName: nameof(ContentView.BackgroundColor),
        returnType: typeof(Color),
        declaringType: typeof(NeoView),
        defaultValue: Colors.Transparent,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty BackgroundGradientProperty = BindableProperty.Create(
        propertyName: nameof(BackgroundGradient),
        returnType: typeof(Gradient),
        declaringType: typeof(NeoView),
        defaultValue: null,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty ShadowBlurProperty = BindableProperty.Create(
        propertyName: nameof(ShadowBlur),
        returnType: typeof(double),
        declaringType: typeof(NeoView),
        defaultValue: 10.0,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty DrawModeProperty = BindableProperty.Create(
        propertyName: nameof(DrawMode),
        returnType: typeof(DrawMode),
        declaringType: typeof(NeoView),
        defaultValue: DrawMode.Flat,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty ShadowDrawModeProperty = BindableProperty.Create(
        propertyName: nameof(NeoControls.ShadowDrawMode),
        returnType: typeof(ShadowDrawMode),
        declaringType: typeof(NeoView),
        defaultValue: ShadowDrawMode.OuterOnly,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
        propertyName: nameof(Elevation),
        returnType: typeof(double),
        declaringType: typeof(NeoView),
        defaultValue: .6,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty ShadowDistanceProperty = BindableProperty.Create(
        propertyName: nameof(ShadowDistance),
        returnType: typeof(double),
        declaringType: typeof(NeoView),
        defaultValue: 9.0,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty LightShadowColorProperty = BindableProperty.Create(
        propertyName: nameof(LightShadowColor),
        returnType: typeof(Color),
        declaringType: typeof(NeoView),
        defaultValue: Colors.White,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty DarkShadowColorProperty = BindableProperty.Create(
        propertyName: nameof(DarkShadowColor),
        returnType: typeof(Color),
        declaringType: typeof(NeoView),
        defaultValue: Colors.Black,
        propertyChanged: OnVisualPropertyChanged);

    public static readonly BindableProperty InnerViewProperty = BindableProperty.Create(
        propertyName: nameof(InnerView),
        returnType: typeof(View),
        declaringType: typeof(NeoView),
        defaultValue: null,
        propertyChanged: OnInnerViewChanged);

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public Gradient BackgroundGradient
    {
        get => (Gradient)GetValue(BackgroundGradientProperty);
        set => SetValue(BackgroundGradientProperty, value);
    }

    public double Elevation
    {
        get => (double)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    public double ShadowDistance
    {
        get => (double)GetValue(ShadowDistanceProperty);
        set => SetValue(ShadowDistanceProperty, value);
    }

    public double ShadowBlur
    {
        get => (double)GetValue(ShadowBlurProperty);
        set => SetValue(ShadowBlurProperty, value);
    }

    public Color LightShadowColor
    {
        get => (Color)GetValue(LightShadowColorProperty);
        set => SetValue(LightShadowColorProperty, value);
    }

    public Color DarkShadowColor
    {
        get => (Color)GetValue(DarkShadowColorProperty);
        set => SetValue(DarkShadowColorProperty, value);
    }

    public View InnerView
    {
        get => (View)GetValue(InnerViewProperty);
        set => SetValue(InnerViewProperty, value);
    }

    public DrawMode DrawMode
    {
        get => (DrawMode)GetValue(DrawModeProperty);
        set => SetValue(DrawModeProperty, value);
    }

    public ShadowDrawMode ShadowDrawMode
    {
        get => (ShadowDrawMode)GetValue(ShadowDrawModeProperty);
        set => SetValue(ShadowDrawModeProperty, value);
    }

    public NeoView() => InitializeComponent();

    protected virtual void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        var surface = args.Surface;
        var canvas = surface.Canvas;

        canvas.Clear();
        using var paint = new SKPaint();

        paint.IsAntialias = true;
        paint.Style = SKPaintStyle.Fill;

        var context = new RenderContext(canvas, paint, args.Info);
        SetPaintColor(context);

        var drawOuterShadow = ShadowDrawMode == ShadowDrawMode.All || ShadowDrawMode == ShadowDrawMode.OuterOnly;
        var drawInnerShadow = ShadowDrawMode == ShadowDrawMode.All || ShadowDrawMode == ShadowDrawMode.InnerOnly;

        PreDraw(context);

        if (drawOuterShadow)
            DrawOuterShadow(context);

        SetPaintColor(context);
        DrawControl(context);

        if (drawInnerShadow)
            DrawInnerShadow(context);
    }

    protected virtual void SetPaintColor(RenderContext context)
    {
        if (BackgroundGradient != null)
            context.Paint.Shader = BackgroundGradient.BuildShader(context);
        else
            context.Paint.Color = BackgroundColor.ToSKColor();
    }

    protected virtual void PreDraw(RenderContext context)
    {
    }

    protected virtual void DrawInnerShadow(RenderContext context)
    {
        var fShadowDistance = Convert.ToSingle(ShadowDistance);
        var darkShadow = Color.FromRgba(DarkShadowColor.Red, DarkShadowColor.Green, DarkShadowColor.Blue, Elevation);
        var drawPadding = ShadowDrawMode == ShadowDrawMode.InnerOnly ?
            0 : Convert.ToSingle(ShadowBlur * 2);

        var diameter = drawPadding * 2;
        var retangleWidth = context.Info.Width - diameter;
        var retangleHeight = context.Info.Height - diameter;

        using var path = CreatePath(retangleWidth, retangleHeight, drawPadding);

        context.Paint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, Convert.ToSingle(ShadowBlur));

        context.Canvas.ClipPath(path);
        context.Paint.Style = SKPaintStyle.Stroke;
        context.Paint.StrokeWidth = fShadowDistance;

        context.Paint.ImageFilter = LightShadowColor.ToSKDropShadow(-fShadowDistance);
        context.Canvas.DrawPath(path, context.Paint);

        context.Paint.ImageFilter = darkShadow.ToSKDropShadow(fShadowDistance);
        context.Canvas.DrawPath(path, context.Paint);
    }

    protected virtual void DrawOuterShadow(RenderContext context)
    {
        var fShadowDistance = Convert.ToSingle(ShadowDistance);
        var darkShadow = Color.FromRgba(DarkShadowColor.Red, DarkShadowColor.Green, DarkShadowColor.Blue, Elevation);
        var drawPadding = Convert.ToSingle(ShadowBlur * 2);

        context.Paint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, Convert.ToSingle(ShadowBlur));

        var diameter = drawPadding * 2;
        var retangleWidth = context.Info.Width - diameter;
        var retangleHeight = context.Info.Height - diameter;

        using var path = CreatePath(retangleWidth, retangleHeight, drawPadding);

        context.Paint.ImageFilter = darkShadow.ToSKDropShadow(fShadowDistance);
        context.Canvas.DrawPath(path, context.Paint);

        context.Paint.ImageFilter = LightShadowColor.ToSKDropShadow(-fShadowDistance);
        context.Canvas.DrawPath(path, context.Paint);
    }

    protected abstract void DrawControl(RenderContext context);

    protected abstract SKPath CreatePath(float retangleWidth, float retangleHeight, float drawPadding);

    protected static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
        ((NeoView)bindable).canvas.InvalidateSurface();

    private static void OnInnerViewChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NeoView neoView)
        {
            if (newValue is View child)
                neoView.rootView.Children.Add(child);
        }
    }
}