using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace NogginAgenda
{	
	public partial class LoadingPage : ContentPage
	{	
        public LoadingPage()
		{
			InitializeComponent();
		}

        public async Task Show(INavigation nav)
        {
            await nav.PushModalAsync(this);
            IsBusy = true; 
        }

        public async Task Hide()
        {
            IsBusy = false;
            await Navigation.PopModalAsync();
        }
	}
}