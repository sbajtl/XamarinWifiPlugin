using WifiSample.ViewModels;
using Xamarin.Forms;

namespace WifiSample
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel vm;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = vm = new MainPageViewModel();
        }
    }
}
