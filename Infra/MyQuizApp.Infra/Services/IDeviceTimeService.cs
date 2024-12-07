namespace MyQuizApp.Infra.Services;

public interface IDeviceTimeService
{
    bool IsAutomaticTimeEnabled();
    bool IsAutomaticTimeZoneEnabled();
}