using System;
using System.ComponentModel;
using CoreGraphics;
using NogginAgenda.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(CircleImage), typeof(NogginAgenda.iOS.Renderers.CircleImageRenderer))]

namespace NogginAgenda.iOS.Renderers
{
    public class CircleImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged (e);

            if (Control == null || Element == null)
                return;

            SetImageCornerRadius();
            Control.Layer.MasksToBounds = true;
            // Todo: Add Border Colour as element property
            //var c = AppConstants.ForegroundThemeColor;
            //Control.Layer.BorderColor = new CGColor ((float)c.R, (float)c.G, (float)c.B);
            Control.Layer.BorderWidth = 3;
            Control.ClipsToBounds = true;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);

            if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
               e.PropertyName == VisualElement.HeightProperty.PropertyName)
            {
                SetImageCornerRadius();
            }
        }

        private void SetImageCornerRadius()
        {
            var min = (float)Math.Min (Element.Width, Element.Height);
            Control.Layer.CornerRadius = min / 2f;
        }
    }
}

