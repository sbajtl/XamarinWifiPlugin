namespace Plugin.Wifi
{
    public enum WiFiSecurityTypes
    {
        Unknown,
        NoAuthentication,
        WEP,
        WPA2Personal,
        WPA2Enterprise,
        Other,
    }

    public enum WorkStatus
    {
        Undefined,
        Working,
        Canceled,
        Failed,
        Finished,
    }
}
