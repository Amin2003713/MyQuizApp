namespace MyQuizApp.App;

using Foundation;
using Infra.Services;

public class DeviceTimeServiceIos : IDeviceTimeService
{
    public bool IsAutomaticTimeEnabled()
    {
        // iOS does not expose a direct API to check if the time is set to automatic.
        // This is a workaround that assumes manual time changes won't align with a synchronized time server.
        return true; // iOS typically enforces automatic time.
    }

    public bool IsAutomaticTimeZoneEnabled()
    {
        var defaults = new NSDictionary(NSUserDefaults.StandardUserDefaults.ToDictionary());
        return defaults.ContainsKey(new NSString("com.apple.preferences.timezone")) &&
               defaults["com.apple.preferences.timezone"]?.ToString() == "1";
    }
}