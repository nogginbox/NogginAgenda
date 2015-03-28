using System;
using System.ComponentModel;
using NogginAgenda.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;


[assembly: ExportRenderer(typeof(CircleImage), typeof(NogginAgenda.Android.Renderers.CircleImageRenderer))]

namespace NogginAgenda.Android.Renderers
{
    // https://gist.github.com/jamesmontemagno/36a25351625ab7e2838e

    public class CircleImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged (e);
            /*if (e.OldElement == null)
            {
                if((int)Android.OS.Build.VERSION.SdkInt < 18)
                    SetLayerType (LayerType.Software, null);
            }*/

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged (sender, e);


        }

        protected override bool DrawChild (Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            try
            {
                var radius = Math.Min (Width, Height) / 2;
                var strokeWidth = 10;
                radius -= strokeWidth / 2;


                var path = new Path();
                path.AddCircle (Width / 2, Height / 2, radius, Path.Direction.Ccw);
                canvas.Save ();
                canvas.ClipPath (path);

                var result = base.DrawChild (canvas, child, drawingTime);

                canvas.Restore ();

                path = new Path ();
                path.AddCircle (Width / 2, Height / 2, radius, Path.Direction.Ccw);

                var paint = new Paint ();
                paint.AntiAlias = true;
                paint.StrokeWidth = 5;
                paint.SetStyle (Paint.Style.Stroke);
                paint.Color = global::Android.Graphics.Color.White;

                canvas.DrawPath (path, paint);

                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch(Exception ex) {
            }

            return base.DrawChild (canvas, child, drawingTime);
        }
    }
}

