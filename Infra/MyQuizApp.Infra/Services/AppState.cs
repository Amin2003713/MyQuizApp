namespace MyQuizApp.Infra.Services;

using Microsoft.AspNetCore.Components;

public class AppState  : IAppState
{
    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                NotifyStateChanged();
            }
        }
    }

    public event Action? OnChange;

    public void ShowLoader()
    {
        IsLoading = true;
        NotifyStateChanged();
    }

    public void HideLoader()
    {
        IsLoading = false;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();


}