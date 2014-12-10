using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace NogginAgenda
{	
	public partial class LoadingPage : ContentPage
	{	
        private readonly CarouselPage _slotsPage;

        public LoadingPage(CarouselPage carouselPage)
		{
			InitializeComponent();
            _slotsPage = carouselPage;
            IsBusy = true;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (App.EventData.Slots.Any ()) return;


            await App.InitEventData();

            // Carousel page needs to be constructed like this,
            // Databinding ItemSource didn't work at time of making
            _slotsPage.Title = "DDD North 2014";
            _slotsPage.ItemsSource = App.EventData.Slots;
            _slotsPage.ItemTemplate =  new DataTemplate(() => {
                return new TalksListPage();
            });

            IsBusy = false;
            await Navigation.PopModalAsync();


            /* Todo: Rethink how we show errors
             * slotsPage.Appearing += (object sender, EventArgs e) => {
                if(!String.IsNullOrEmpty(_errorMessage))
                {
                    var errorTitle = (EventData == null)
                        ? "Error"
                        : "Warning";

                    (sender as Page).DisplayAlert(errorTitle, _errorMessage, "OK");
                    // Todo: can cancel when no data close the app
                }
            };*/
        }
	}
}

