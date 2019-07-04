using Android;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
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
        // internal static Context AppContext;
        private WifiManager NetworkManager { get; set; }
        private WifiReceiver NetworkReceiver { get; set; }

        public static void Init(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Init(activity, savedInstanceState);
        }

        /// <summary>
        /// GetWifiList
        /// </summary>
        /// <returns></returns>
        public async Task<IList<WifiInfo>> GetWifiList()
        {
            IList<WifiInfo> networks = new List<WifiInfo>();

            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();

                if (status != PermissionStatus.Granted)
                {
                    bool shouldShow = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location);

                    if (shouldShow)
                    {
                        await WifiHelper.DisplayCustomDialog(CrossCurrentActivity.Current.Activity, "Permission needed", "You have to grant location permission to get wifi list!", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    networks = await ScanAndGetNetworks();
                }
            }
            catch (Exception ex)
            {
                // Something went wrong
                System.Diagnostics.Debug.WriteLine($"Error when trying to get wifi list: {ex.Message}");
            }

            return networks;
        }

        /// <summary>
        /// ScanAndGetNetworks
        /// </summary>
        /// <returns></returns>
        private async Task<IList<WifiInfo>> ScanAndGetNetworks()
        {
            await ScanForNetworks();
            return await NetworkReceiver.GetAvailableWifiNetworks();
        }

        /// <summary>
        /// ScanForNetworks
        /// </summary>
        /// <returns></returns>
        private async Task ScanForNetworks()
        {
            await Task.Run(() =>
            {
                if (NetworkManager == null)
                {
                    NetworkManager = (WifiManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.WifiService);
                }
                    
                NetworkReceiver = new WifiReceiver(NetworkManager);
                IntentFilter intentFilter = new IntentFilter();
                intentFilter.AddAction(WifiManager.ScanResultsAvailableAction);
                CrossCurrentActivity.Current.Activity.RegisterReceiver(NetworkReceiver, intentFilter);
                NetworkManager.StartScan();
            });
    
        }

        /// <summary>
        /// WifiReceiver
        /// </summary>
        private class WifiReceiver : BroadcastReceiver
        {
            public WifiReceiver(WifiManager networkManager)
            {
                WifiListCompletion = new TaskCompletionSource<IList<WifiInfo>>();
                WifiManager = networkManager;
            }

            private WifiManager WifiManager { get; set; }

            private IList<WifiInfo> WifiNetworks { get; set; }

            private TaskCompletionSource<IList<WifiInfo>> WifiListCompletion { get; set; }

            /// <summary>
            /// OnReceive
            /// </summary>
            /// <param name="context"></param>
            /// <param name="intent"></param>
            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> foundedNetworks = WifiManager.ScanResults;

                WifiNetworks = new List<WifiInfo>();

                foreach (ScanResult wifiResult in foundedNetworks)
                {
                    if (string.IsNullOrEmpty(wifiResult.Ssid))
                    {
                        continue;
                    } // hidden network

                    WifiInfo wifiInfo = WifiHelper.MakeWiFiInfo(wifiResult);

                    if (!WifiNetworks.Contains(wifiInfo))
                    {
                        bool canAdd = true;
                        foreach (WifiInfo wifi in WifiNetworks)
                        {
                            if (wifi.Identity == wifiResult.Bssid)
                            {
                                canAdd = false;
                                break;
                            }
                        }

                        if (canAdd)
                        {
                            WifiNetworks.Add(wifiInfo);
                        }
                    }
                }

                WifiListCompletion.TrySetResult(WifiNetworks);
            }

            /// <summary>
            /// GetAvailableWifiNetworks
            /// </summary>
            /// <returns></returns>
            public async Task<IList<WifiInfo>> GetAvailableWifiNetworks()
            {
                return await WifiListCompletion.Task;
            }
        }
    }
}
