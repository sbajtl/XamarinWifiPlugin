using Android;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

[assembly: UsesPermission(Manifest.Permission.AccessWifiState)]
[assembly: UsesPermission(Manifest.Permission.AccessFineLocation)]
[assembly: UsesPermission(Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Manifest.Permission.ChangeWifiState)]
namespace Plugin.Wifi
{
    /// <summary>
    /// Interface for Wifi
    /// </summary>
    public class WifiPlugin : IWifi
    {
        internal static Context AppContext;
        private static WifiManager wifiManager;
        private static WifiReceiver wifiReceiver;
        private static IList<WifiInfo> wifiNetworks;

        public static void Init(Context context)
        {
            AppContext = context;
        }

        public async Task<IList<WifiInfo>> GetWifiList()
        {
            IList <WifiInfo> networks = new List<WifiInfo>();

            if (PermissionsHandler.NeedsPermissionRequest((Activity)AppContext))
            {
               await PermissionsHandler.RequestPermissionsAsync((Activity)AppContext);
               networks = GetNetworks();
            }
            else
            {
                networks = GetNetworks();
            }

            return await Task.FromResult(networks);
        }

        private static IList<WifiInfo> GetNetworks()
        {
            IList<WifiInfo> networks;
            if (wifiManager == null)
            {
                wifiManager = (WifiManager)AppContext.GetSystemService(Context.WifiService);
            }

            if (wifiReceiver == null)
            {
                wifiReceiver = new WifiReceiver();
            }

            IntentFilter intentFilter = new IntentFilter();
            intentFilter.AddAction(WifiManager.ScanResultsAvailableAction);
            AppContext.RegisterReceiver(wifiReceiver, intentFilter);
            wifiManager.StartScan();
            networks = wifiNetworks;
            return networks;
        }

        private class WifiReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> foundedNetworks = wifiManager.ScanResults;

                wifiNetworks = new List<WifiInfo>();

                foreach (ScanResult item in foundedNetworks)
                {
                    //wifiNetworks.Add(new WifiInfo
                    //{
                    //    //item.
                    //});
                }
            }
        }
    }
}
