using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NogginAgenda
{	
	public partial class TalkDescriptionPage : ContentPage
	{	
		public TalkDescriptionPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing ()
        {
            TwitterLink.IsVisible = !String.IsNullOrEmpty(TwitterLink.Uri);
            base.OnAppearing ();
        }
	}
}

