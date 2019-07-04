using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Wifi
{
    public class WifiInfo
    {
        private int signal = -1;

        // public int Id { get; set; }

        public string Identity { get; set; }

        public string Key { get; set; }

        public string Ssid { get; set; }

        public WiFiSecurityTypes Security { get; set; }

        public int Signal
        {
            get
            {
                return signal;
            }

            set
            {
                if (signal != value)
                {
                    if (value < -1)
                    {
                        signal = -1;
                    }
                    else if (value > 4)
                    {
                        signal = 4;
                    }
                    else
                    {
                        signal = value;
                    }
                }
            }
        }
    }
}
