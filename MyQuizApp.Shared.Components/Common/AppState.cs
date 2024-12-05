namespace MyQuizApp.Shared.Components.Common;

public class AppState
{
    public event Action? OnChange;

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading == value) return;
            _isLoading = value;
            NotifyStateChanged();
        }
    }

    public void ShowLoader()
    {
        IsLoading = true;
    }

    public void HideLoader()
    {
        IsLoading = false;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}