using SkiaSharp;

namespace Maui.NeoControls
{
    public class LinearGradient : Gradient
    {
        #region Fields

        private readonly LinearGradientRenderer _renderer;

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty AngleProperty = BindableProperty.Create(
            propertyName: nameof(Angle),
            returnType: typeof(double),
            declaringType: typeof(LinearGradient),
            defaultValue: 0d);

        #endregion

        #region Properties

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        #endregion

        #region Constructors

        public LinearGradient() =>
            _renderer = new LinearGradientRenderer(this);

        #endregion

        #region Override Methods

        public override void Measure(int width, int height)
        {
            base.Measure(width, height);

            foreach (var stop in Stops)
            {
                if (stop.RenderOffset > 1.001)
                    stop.RenderOffset = GetOffsetFromPixels(stop.RenderOffset, width, height);
            }
        }

        public override SKShader BuildShader(RenderContext context)
        {
#if DEBUG_RENDER
            System.Diagnostics.Debug.WriteLine($"Rendering Linear Gradient with {Stops.Count} stops");
#endif
            return _renderer.BuildShader(context);
        }

        #endregion

        #region Private Methods

        private float GetOffsetFromPixels(float offset, int width, int height)
        {
            var angleRad = (Math.PI / 180) * Angle;
            var computedLength = Math.Sqrt(Math.Pow(width * Math.Cos(angleRad), 2)
                + Math.Pow(height * Math.Sin(angleRad), 2));

            return computedLength != 0 ? (float)(offset / computedLength) : 1;
        }

        #endregion
    }
}
