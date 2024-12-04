using MyQuizApp.Infra.Services;

namespace MyQuizApp.Web.Services;

using Blazored.LocalStorage;
using Infra.Common;
using Infra.Users;
using Infra.Users.Response;

public class FormFactor : IFormFactor
{
    public string GetFormFactor()
    {
        return "WebAssembly";
    }

    public string GetPlatform()
    {
        return Environment.OSVersion.ToString();
    }
}

public class WebLocalStorage(ILocalStorageService storageService) : ILocalStorage
{
    public async ValueTask SetItemAsync<TType>(string key, TType value)
    {
        await storageService.SetItemAsync(UserConst.UserInfo, value);
    }

    public async ValueTask<TType?> GetItemAsync<TType>(string key)
    {
        return await storageService.GetItemAsync<TType>(UserConst.UserInfo);
    }

    public async ValueTask RemoveItemAsync(string key)
    {
        await storageService.RemoveItemAsync(UserConst.UserInfo);
    }

    public ValueTask ClearAsync()
    {
        throw new NotImplementedException();
    }
}