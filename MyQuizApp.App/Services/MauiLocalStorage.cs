namespace MyQuizApp.App.Services;

using System.Text.Json;
using Infra.Common;

public class MauiLocalStorage : ILocalStorage
{
    public ValueTask SetItemAsync<TType>(string key, TType value)
    {
        Preferences.Default.Set(key, value);

        return ValueTask.CompletedTask;
    }

    public  ValueTask<TType?> GetItemAsync<TType>(string key) 
        =>  ValueTask.FromResult(Preferences.Default.Get<TType>(key, Activator.CreateInstance<TType>()!))!;

    public ValueTask RemoveItemAsync(string key)
    {
        Preferences.Remove(key);
        return ValueTask.CompletedTask;
    }

    public ValueTask ClearAsync()
    {
        Preferences.Clear();
        return ValueTask.CompletedTask;
    }
}