using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace Plugin.Wifi
{
    /// <summary>
    /// Interface for Wifi
    /// </summary>
    public class WifiPlugin : IWifi
    {
        public Task<IList<WifiInfo>> GetWifiList()
        {
            var WiFiURL = new NSUrl("prefs:root=WIFI");
            if (UIApplication.SharedApplication.CanOpenUrl(WiFiURL))
            {   //Pre iOS 10
                UIApplication.SharedApplication.OpenUrl(WiFiURL);
            }
            else
            {   //iOS 10
                UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=WIFI"));
            }

            return null;
        }
    }
}
