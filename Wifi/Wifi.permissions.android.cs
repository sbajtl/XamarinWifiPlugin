using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Wifi
{
    public static class PermissionsHandler
    {
        public const int RequestWifiId = 0;

        public static readonly string[] PermissionsGroupWifi = new string[] 
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
        };

    }
}
