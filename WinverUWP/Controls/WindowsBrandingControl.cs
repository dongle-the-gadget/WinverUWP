using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using WinverUWP.Helpers;

namespace WinverUWP.Controls;

public enum WindowsBranding
{
    Windows10,
    Windows11
}

public partial class WindowsBrandingControl : FrameworkElement
{
    Compositor compositor;
    ShapeVisual visual;
    
    public WindowsBranding Branding
    {
        get => (WindowsBranding)GetValue(BrandingProperty);
        set => SetValue(BrandingProperty, value);
    }

    public static DependencyProperty BrandingProperty = DependencyProperty.Register(
        "Branding",
        typeof(WindowsBranding),
        typeof(WindowsBrandingControl),
        new PropertyMetadata(WindowsBranding.Windows11, OnBrandingChanged));

    public SolidColorBrush Foreground
    {
        get => (SolidColorBrush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static DependencyProperty ForegroundProperty = DependencyProperty.Register(
        "Foreground",
        typeof(SolidColorBrush),
        typeof(WindowsBrandingControl),
        new PropertyMetadata(null, OnForegroundChanged));

    private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var newValue = (SolidColorBrush)e.NewValue;
        var control = (WindowsBrandingControl)d;

        var shape = control.visual.Shapes.FirstOrDefault();
        if (shape is CompositionSpriteShape sprite)
        {
            var oldBrush = sprite.FillBrush;
            sprite.FillBrush = control.compositor.CreateColorBrush(newValue.Color);
            oldBrush?.Dispose();
        }
    }

    private static void OnBrandingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var newValue = (WindowsBranding)e.NewValue;
        var control = (WindowsBrandingControl)d;
        control.visual.Shapes.Clear();

        var branding = GetPathForOS(newValue);
        CompositionPath path = new(branding.Geometry);
        CompositionPathGeometry compGeo = control.compositor.CreatePathGeometry(path);
        var shape = control.compositor.CreateSpriteShape(compGeo);
        if (control.Foreground is not null)
            shape.FillBrush = control.compositor.CreateColorBrush(control.Foreground.Color);
        control.visual.Shapes.Add(shape);
        control.visual.Size = new(branding.Width, branding.Height);

        control.InvalidateMeasure();
    }

    public WindowsBrandingControl()
    {
        compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        visual = compositor.CreateShapeVisual();
        ElementCompositionPreview.SetElementChildVisual(this, visual);
    }

    private static OSPath GetPathForOS(WindowsBranding branding)
        => branding == WindowsBranding.Windows11 ? OSPathsHelper.GetWindows11Path() : OSPathsHelper.GetWindows10Path();

    protected override Size MeasureOverride(Size availableSize)
    {
        var path = GetPathForOS(Branding);
        if (availableSize.Width == double.PositiveInfinity && availableSize.Height == double.PositiveInfinity)
        {
            return new(path.Width, path.Height);
        }

        double widthRatio = availableSize.Width / path.Width;
        double heightRatio = availableSize.Height / path.Height;

        double minRatio = double.Min(widthRatio, heightRatio);
        return new Size(minRatio * path.Width, minRatio * path.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var path = GetPathForOS(Branding);

        double widthRatio = finalSize.Width / path.Width;
        double heightRatio = finalSize.Height / path.Height;

        double minRatio = double.Min(widthRatio, heightRatio);

        visual.Scale = new((float)minRatio);
        
        float widthOffset = (float)(finalSize.Width - minRatio * path.Width) / 2;
        float heightOffset = (float)(finalSize.Height - minRatio * path.Height) / 2;

        visual.Offset = new(widthOffset, heightOffset, 1f);

        return finalSize;
    }
}