using Plugin.Wifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WifiSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            addButton.Clicked += AddButton_Clicked;
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            var source = await CrossWifi.Current.GetWifiList();
            wifiList.ItemsSource = source;
        }
    }
}
