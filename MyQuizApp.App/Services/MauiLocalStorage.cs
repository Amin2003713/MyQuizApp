namespace MyQuizApp.App.Services;

using System.Text.Json;
using Infra.Common;

public class MauiLocalStorage : ILocalStorage
{
    public ValueTask SetItemAsync<TType>(string key, TType value)
    {
        // Serialize the object to a JSON string
        var jsonValue = JsonSerializer.Serialize(value);
        Preferences.Default.Set(key, jsonValue);

        return ValueTask.CompletedTask;
    }

    public ValueTask<TType?> GetItemAsync<TType>(string key)
    {
        // Retrieve the JSON string and deserialize it
        var jsonValue = Preferences.Default.Get<string>(key, null!);
        if (string.IsNullOrEmpty(jsonValue))
            return ValueTask.FromResult(default(TType));

        var value = JsonSerializer.Deserialize<TType>(jsonValue);
        return ValueTask.FromResult(value);
    }

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