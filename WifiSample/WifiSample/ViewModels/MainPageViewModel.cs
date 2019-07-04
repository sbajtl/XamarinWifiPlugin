using Plugin.Wifi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace WifiSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IList<WifiInfo> wifiList;
        public Command WifiListCommand { get; set; }

        public MainPageViewModel()
        {
            WifiListCommand = new Command(GetWifiList);
            WifiList = new List<WifiInfo>();
        }

        private async void GetWifiList(object obj)
        {
            if(WifiList.Count > 0)
            {
                WifiList.Clear();
            }

            WifiList = await CrossWifi.Current.GetWifiList();
        }

        public IList<WifiInfo> WifiList
        {
            get
            {
                return wifiList;
            }

            set
            {
                SetProperty(ref wifiList, value);
            }
        }
    }
}
