namespace MyQuizApp.Infra.Common;

public interface ILocalStorage
{
    ValueTask SetItemAsync<TType>(string key, TType value);
    ValueTask<TType?> GetItemAsync<TType>(string key);
    ValueTask RemoveItemAsync(string key);
    ValueTask ClearAsync();
}