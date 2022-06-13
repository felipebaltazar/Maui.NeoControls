using Maui.NeoControls.Abstractions;

namespace Maui.NeoControls
{
    public class NeoGradientStop : BindableObject, IWithParentElement, IWithCanvasElement
    {
        #region Bindable Properties

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            propertyName: nameof(Color),
            returnType: typeof(Color),
            declaringType: typeof(NeoGradientStop),
            defaultValue: Colors.White);

        public static readonly BindableProperty OffsetProperty = BindableProperty.Create(
            propertyName: nameof(Offset),
            returnType: typeof(float),
            declaringType: typeof(NeoGradientStop),
            defaultValue: -1f,
            propertyChanged: OnOffsetChanged);

        #endregion

        #region Properties

        public BindableObject? Parent { get; set; }

        public float RenderOffset { get; set; } = -1f;

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public float Offset
        {
            get => (float)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        #endregion

        #region IWithCanvasElement

        public void InvalidateCanvas()
        {
            if (Parent is IWithCanvasElement parentWithCanvas)
                parentWithCanvas?.InvalidateCanvas();
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return $"Offset={Offset}, Color=[{Color}]";
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            InvalidateCanvas();
        }

        #endregion

        #region Private Methods

        private static void OnOffsetChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((NeoGradientStop)bindable).RenderOffset = (float)newvalue;
        }

        #endregion
    }
}
