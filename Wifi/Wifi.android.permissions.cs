using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Wifi
{
    public static class PermissionsHandler
    {
        private const int RequestWifiId = 0;
        static TaskCompletionSource<bool> requestCompletion = null;
        private static readonly Activity activity;

        public static Task PermissionRequestTask
        {
            get
            {
                return requestCompletion?.Task ?? Task.CompletedTask;
            }
        }

        private static readonly string[] PermissionsGroupWifi = new string[] 
        {
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.ChangeWifiState,
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool NeedsPermissionRequest(Context context)
        {
            var permissionsToRequest = new List<string>();

            // Check and request any permissions
            foreach (var permission in PermissionsGroupWifi)
            {
                if (IsPermissionInManifest(context, permission))
                {
                    if (!IsPermissionGranted(context, permission))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        internal static bool IsPermissionGranted(Context context, string permission)
        {
            return context.CheckSelfPermission(permission) == Permission.Granted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static Task<bool> RequestPermissionsAsync(Activity activity)
        {
            if (requestCompletion != null && !requestCompletion.Task.IsCompleted)
                return requestCompletion.Task;

            var permissionsToRequest = new List<string>();

            // Check and request any permissions
            foreach (var permission in PermissionsGroupWifi)
            {
                if (IsPermissionInManifest(activity, permission))
                {
                    if (!IsPermissionGranted(activity, permission))
                        permissionsToRequest.Add(permission);
                }
            }

            if (permissionsToRequest.Any())
            {
                DoRequestPermissions(activity, permissionsToRequest.ToArray(), RequestWifiId);
                requestCompletion = new TaskCompletionSource<bool>();

                return requestCompletion.Task;
            }

            return Task.FromResult<bool>(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        internal static bool IsPermissionInManifest(Context context, string permission)
        {
            try
            {
                PackageInfo info = context.PackageManager.GetPackageInfo(context.PackageName, PackageInfoFlags.Permissions);
                return info.RequestedPermissions.Contains(permission);
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="permissions"></param>
        /// <param name="requestCode"></param>
        /// <returns></returns>
        internal static bool DoRequestPermissions(Activity activity, string[] permissions, int requestCode)
        {
            var permissionsToRequest = new List<string>();
            foreach (var permission in permissions)
            {
                if (activity.CheckSelfPermission(permission) != Permission.Granted)
                    permissionsToRequest.Add(permission);
            }

            if (permissionsToRequest.Any())
            {
                activity.RequestPermissions(permissionsToRequest.ToArray(), requestCode);
                return true;
            }

            return false;
        }
    }
}
