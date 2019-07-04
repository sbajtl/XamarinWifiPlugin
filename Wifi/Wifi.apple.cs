using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Wifi
{
    /// <summary>
    /// Interface for Wifi
    /// </summary>
    public class WifiPlugin : IWifi
    {
        public Task<IList<WifiInfo>> GetWifiList()
        {
            throw new NotImplementedException();
        }
    }
}
