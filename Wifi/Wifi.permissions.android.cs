using Android;

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
