using Microsoft.Phone.Controls;
using Xamarin.Forms;

namespace NogginAgenda.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Forms.Init();
            Content = NogginAgenda.App.GetMainPage().ConvertPageToUIElement(this);
        }
    }
}