using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NogginAgenda
{	
	public partial class TalksListPage : ContentPage
	{	
		private TalkDescriptionPage _talkPage;

		public TalksListPage()
		{
			InitializeComponent ();
			_talkPage = new TalkDescriptionPage ();
		}

		protected void OnTalkSelected(object sender, SelectedItemChangedEventArgs args)
		{
			if (args.SelectedItem == null) return;

			_talkPage.BindingContext = args.SelectedItem;
			Navigation.PushAsync(_talkPage);

			((ListView)sender).SelectedItem = null;
		}
	}
}

