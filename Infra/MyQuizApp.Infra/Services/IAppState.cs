namespace MyQuizApp.Infra.Services;

public interface IAppState
{
    public bool IsLoading { get; }
    public event Action? OnChange;

    void ShowLoader();
    void HideLoader();
}