namespace MyQuizApp.App;

using Foundation;
using Infra.Services;

public class DeviceTimeServiceMac : IDeviceTimeService
{
    public bool IsAutomaticTimeEnabled()
    {
        // macOS does not expose a direct API for this, but we can assume it's enabled for most setups.
        return true;
    }

    public bool IsAutomaticTimeZoneEnabled()
    {
        var defaults = new NSDictionary(NSUserDefaults.StandardUserDefaults.ToDictionary());
        return defaults.ContainsKey(new NSString("com.apple.preferences.timezone")) &&
               defaults["com.apple.preferences.timezone"]?.ToString() == "1";
    }
}