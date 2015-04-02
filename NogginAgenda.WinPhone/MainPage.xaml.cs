using Microsoft.Phone.Controls;
using Xamarin.Forms;

namespace NogginAgenda.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new NogginAgenda.App());
        }
    }
}

/*
public partial class MainPage : 
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            
        }
    }

*/