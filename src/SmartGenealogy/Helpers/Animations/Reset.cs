using AlohaKit.Animations;

namespace SmartGenealogy.Helpers;

public class Reset : AnimationBase
{
    public static BindableProperty OpacityProperty =
        BindableProperty.Create(
            nameof(Opacity),
            typeof(double?),
            typeof(Reset),
            null);

    public double? Opacity
    {
        get => (double?)GetValue(OpacityProperty);
        set => SetValue(OpacityProperty, value);
    }

    public static BindableProperty RotationProperty =
        BindableProperty.Create(
            nameof(Rotation),
            typeof(double?),
            typeof(Reset),
            null);

    public double? Rotation
    {
        get => (double?)GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    public static BindableProperty ScaleProperty =
        BindableProperty.Create(
            nameof(Scale),
            typeof(double?),
            typeof(Reset),
            null);

    public double? Scale
    {
        get => (double?)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static BindableProperty TranslateXProperty =
        BindableProperty.Create(
            nameof(TranslateX),
            typeof(double?),
            typeof(Reset),
            null);

    public double? TranslateX
    {
        get => (double?)GetValue(TranslateXProperty);
        set => SetValue(TranslateXProperty, value);
    }

    public static BindableProperty TranslateYProperty =
        BindableProperty.Create(
            nameof(TranslateY),
            typeof(double?),
            typeof(Reset),
            null);

    public double? TranslateY
    {
        get => (double?)GetValue(TranslateYProperty);
        set => SetValue(TranslateYProperty, value);
    }

    protected override Task BeginAnimation()
    {
        if (Target != null)
        {
            if (Opacity is double opacity)
            {
                Target.Opacity = opacity;
            }

            if (Rotation is double rotation)
            {
                Target.Rotation = rotation;
            }

            if (Scale is double scale)
            {
                Target.Scale = scale;
            }

            if (TranslateX is double translateX)
            {
                Target.TranslationX = translateX;
            }

            if (TranslateY is double translateY)
            {
                Target.TranslationY = translateY;
            }
        }

        return Task.CompletedTask;
    }
}