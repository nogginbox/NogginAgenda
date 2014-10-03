using System;
using Xamarin.Forms;

namespace NogginAgenda.Xaml
{
    public class LinkButton : Button
    {
        public LinkButton ()
        {
            this.Clicked += (object sender, EventArgs e) => {
                Device.OpenUri(new Uri(Uri));
            };
        }

        // Bindable property for the link Uri
        public static readonly BindableProperty UriProperty =
            BindableProperty.Create<LinkButton, String> (p => p.Uri, "");

        //Gets or sets the link uri
        public String Uri
        {
            get { return (String)GetValue (UriProperty); }
            set
            {
                this.IsVisible = !String.IsNullOrEmpty (value);
                SetValue (UriProperty, value);
            }
        }
    }
}