using Maui.NeoControls.Extensions;
using System.Windows.Input;

namespace Maui.NeoControls;

public class NeoButton : NeoRoundedView
{
    private bool isPerformingTap;

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        propertyName: nameof(Command),
        returnType: typeof(ICommand),
        declaringType: typeof(NeoButton),
        defaultValue: null);

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        propertyName: nameof(CommandParameter),
        returnType: typeof(object),
        declaringType: typeof(NeoButton),
        defaultValue: null);

    public static readonly BindableProperty ClickModeProperty = BindableProperty.Create(
        propertyName: nameof(ClickMode),
        returnType: typeof(ClickMode),
        declaringType: typeof(NeoButton),
        defaultValue: ClickMode.SingleTap);

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        propertyName: nameof(IsChecked),
        returnType: typeof(bool),
        declaringType: typeof(NeoButton),
        defaultValue: false,
        propertyChanged: OnIsCheckedChanged);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public ClickMode ClickMode
    {
        get => (ClickMode)GetValue(ClickModeProperty);
        set => SetValue(ClickModeProperty, value);
    }

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public event EventHandler? Clicked;

    public NeoButton()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnButtonTapped;
        GestureRecognizers.Add(tapGesture);
    }

    public virtual Task<bool> AnimateClick(double toValue, uint length = 250, Easing? easing = null)
    {
        double transform(double t) => (t * (toValue));
        return PressAnimation(nameof(AnimateClick), transform, length, easing);
    }

    protected virtual async Task PerformButtonTappedAsync()
    {
        if (isPerformingTap || !IsEnabled)
            return;

        isPerformingTap = true;

        try
        {
            Clicked?.Invoke(this, EventArgs.Empty);

            if (Command?.CanExecute(CommandParameter) ?? false)
                Command.Execute(CommandParameter);

            if (ClickMode == ClickMode.Toggle)
            {
                IsChecked = !IsChecked;
                return;
            }

            await AnimateClick(ShadowDistance * -1);
            await AnimateClick(ShadowDistance * -1);
        }
        finally
        {
            isPerformingTap = false;
        }
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
    }

    protected virtual Task<bool> PressAnimation(string name, Func<double, double> transform, uint length, Easing? easing)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();
        (this).Animate(
            name,
            transform,
            (distance) => ShadowDistance = distance,
            8,
            length,
            easing ?? Easing.Linear,
            (v, c) => taskCompletionSource.SetResult(c));

        return taskCompletionSource.Task;
    }

    private void OnButtonTapped(object? sender, EventArgs e) =>
        PerformButtonTappedAsync().SafeFireAndForget();

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NeoButton neoButton)
            PerformIsCheckedAnimationAsync(neoButton).SafeFireAndForget();
    }

    private static async Task PerformIsCheckedAnimationAsync(NeoButton neoButton) =>
        await neoButton.AnimateClick(neoButton.ShadowDistance * -1);
}
