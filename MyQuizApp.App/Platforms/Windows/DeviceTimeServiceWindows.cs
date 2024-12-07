namespace MyQuizApp.App.WinUI;

using Infra.Services;
using Microsoft.Win32;

public class DeviceTimeServiceWindows : IDeviceTimeService
{
    public bool IsAutomaticTimeEnabled()
    {
        const string keyPath = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Parameters";
        var          value   = Registry.GetValue(keyPath, "ServiceDll", null);
        return value != null;
    }

    public bool IsAutomaticTimeZoneEnabled()
    {
        const string keyPath =
            @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\DateTime\AutomaticTimeZone";
        var value = Registry.GetValue(keyPath, "Enabled", null);
        return value != null && value.ToString() == "1";
    }
}