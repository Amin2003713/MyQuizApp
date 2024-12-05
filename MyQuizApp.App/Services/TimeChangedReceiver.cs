namespace MyQuizApp.App.Services;

using Android.App;
using Android.Content;
using Android.Provider;
using Android.Util;
using Android.Widget;

[BroadcastReceiver(Enabled = true , Exported = true )]
[IntentFilter([Intent.ActionTimeChanged, Intent.ActionTimezoneChanged])]
public class TimeChangedReceiver : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        Log. Info("TimeChangedReceiver", $"Time changed detected: {intent?.Action}");

        if(!IsAutoTimeZoneEnabled())
            Toast.MakeText(context, "Device time was modified!", ToastLength.Short)?.Show();
    }

    public bool IsAutoTimeZoneEnabled()
    {
        var isAutoTimeZone = Settings.Global.GetInt(
            Android.App.Application.Context.ContentResolver
            , Settings.Global.AutoTimeZone
            , 0) == 1;

        var isAutoTime1Zone = Settings.Global.GetInt(
            Android.App.Application.Context.ContentResolver
            , Settings.Global.AutoTime
            , 0) == 1;

        var isAutoTime21Zone = Settings.Global.GetInt(
            Android.App.Application.Context.ContentResolver
            , Settings.Global.DevelopmentSettingsEnabled
            , 0) == 1;

        return isAutoTimeZone && isAutoTime1Zone;
    }
}