using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Wifi
{
    public interface IWifi
    {
        Task<IList<WifiInfo>> GetWifiList();
    }
}
