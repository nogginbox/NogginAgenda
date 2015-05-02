using System;
using System.ComponentModel;
using System.Windows.Media;
using NogginAgenda.CustomControls;
using NogginAgenda.WinPhone.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]

namespace NogginAgenda.WinPhone.Renderers
{
    public class CircleImageRenderer : ImageRenderer
    {
        // Source: http://blog.xamarin.com/elegant-circle-images-in-xamarin.forms/

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null || Control.Clip != null) return;


            var min = Math.Min(Element.Width, Element.Height) / 2.0f;
            if (min <= 0)
                return;
            Control.Clip = new EllipseGeometry
            {
                Center = new System.Windows.Point(min, min),
                RadiusX = min,
                RadiusY = min
            };
        }
    }
}

