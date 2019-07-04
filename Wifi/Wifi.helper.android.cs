using Android.App;
using Android.Content;
using Android.Net.Wifi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Wifi
{
    public static class WifiHelper
    {
        public static Task<string> DisplayCustomDialog(Context context, string dialogTitle, string dialogMessage, string dialogPositiveBtnLabel, string dialogNegativeBtnLabel = null)
        {
            var tcs = new TaskCompletionSource<string>();

            AlertDialog.Builder alert = new AlertDialog.Builder(context);
            alert.SetTitle(dialogTitle);
            alert.SetMessage(dialogMessage);

            alert.SetPositiveButton(dialogPositiveBtnLabel, (senderAlert, args) =>
            {
                tcs.SetResult(dialogPositiveBtnLabel);
            });

            alert.SetNegativeButton(dialogNegativeBtnLabel, (senderAlert, args) =>
            {
                tcs.SetResult(dialogNegativeBtnLabel);
            });

            Dialog dialog = alert.Create();
            dialog.SetCanceledOnTouchOutside(false);
            dialog.Show();

            return tcs.Task;
        }

        public static WifiInfo MakeWiFiInfo(ScanResult wiFi)
        {
            return MakeWiFiInfo(wiFi.Capabilities, wiFi.Level, wiFi.Bssid, wiFi.Ssid);
        }

        public static WifiInfo MakeWiFiInfo(string capabilities, int signal, string bssid, string ssid)
        {
            WiFiSecurityTypes wiFiSecurity = WiFiSecurityTypes.Other;

            if (!string.IsNullOrEmpty(capabilities))
            {
                // string mCapabilities = capabilities;
                try
                {
                    if (capabilities.Contains("WEP"))
                    {
                        wiFiSecurity = WiFiSecurityTypes.WEP;
                    }
                    else if (capabilities.Contains("WPA") || capabilities.Contains("WPA2"))
                    {
                        if (capabilities.Contains("ENTERPRISE_CAPABILITY"))
                        {
                            wiFiSecurity = WiFiSecurityTypes.WPA2Enterprise;
                        }
                        else
                        {
                            wiFiSecurity = WiFiSecurityTypes.WPA2Personal;
                        }
                    }
                    else if (capabilities.Contains("WPA_EAP"))
                    {
                        wiFiSecurity = WiFiSecurityTypes.WPA2Enterprise;
                    }
                    else if (capabilities.Contains("IEEE8021X"))
                    {
                        wiFiSecurity = WiFiSecurityTypes.Other;
                    }
                    else
                    {
                        wiFiSecurity = WiFiSecurityTypes.NoAuthentication;
                    }
                }
                catch
                {
                }
            }

            int signalStrength = WifiManager.CalculateSignalLevel(signal, 5);
            if (signalStrength < -1)
            {
                signalStrength = 0;
            }

            if (signalStrength > 4)
            {
                signalStrength = 4;
            }

            return new WifiInfo
            {
                // ID = wiFiNetworks.Count(),
                Ssid = ssid,
                Security = wiFiSecurity,
                Identity = bssid,
                Signal = signalStrength,
            };
        }
    }
}
