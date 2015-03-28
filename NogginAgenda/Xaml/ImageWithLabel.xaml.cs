using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NogginAgenda.Xaml
{
    public partial class ImageWithLabel : ContentView
    {
        public ImageWithLabel ()
        {
            InitializeComponent ();
        }

        #region Backing properties

        public static readonly BindableProperty LabelProperty =
            BindableProperty.Create<ImageWithLabel, String> (p => p.Label, "");

        public static readonly BindableProperty LabelBackgroundColorProperty =
            BindableProperty.Create<ImageWithLabel, Color> (p => p.LabelBackgroundColor, Color.White);

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create<ImageWithLabel, Uri> (p => p.Source, null);

        #endregion

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set
            {
                //LabelView.IsVisible = !String.IsNullOrEmpty (value);
                SetValue (LabelProperty, value);
            }
        }

        public Color LabelBackgroundColor
        {
            get { return (Color)GetValue(LabelBackgroundColorProperty); }
            set
            {
                SetValue (LabelBackgroundColorProperty, value);
            }
        }

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set
            {
                SetValue (SourceProperty, value);
            }
        }
    }
}