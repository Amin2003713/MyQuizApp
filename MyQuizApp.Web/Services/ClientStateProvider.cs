using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyQuizApp.Infra.Users;
using MyQuizApp.Infra.Users.Requests;
using MyQuizApp.Infra.Users.Response;

namespace MyQuizApp.Web.Services;


public class ClientStateProvider(
    ILocalStorageService storageService,
    IUserApiClient userApiClient )
    : AuthenticationStateProvider
{

private Task<AuthenticationState> _authenticationStateTask  = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));


public LoggedUser? LoggedUser { get; private set; }
     public bool IsLoggedIn => LoggedUser != null || this.LoggedUser?.Id != Guid.Empty;


    public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
    {
        var state = await GetAuthenticationStateAsync();
        var authenticationStateProviderUser = state.User;
        return authenticationStateProviderUser;
    }


    public override  Task<AuthenticationState> GetAuthenticationStateAsync()
        => _authenticationStateTask;


    public  async Task<LoginResponse> LoginAsync(LoginRequests requests)
    {
        var result  = await userApiClient.Login(requests);

        if (result.HasError)
            return result;

        await storageService.SetItemAsync(UserConst.UserInfo, result.Token);
        LoggedUser = result.Token;

        
         SetAuthenticatedUser(result.Token);
        NotifyAuthenticationStateChanged(_authenticationStateTask);

        return result;
    }

    public async Task LogoutAsync()
    {
        if(IsLoggedIn)
        {
            LoggedUser = null;
            await storageService.RemoveItemAsync(UserConst.UserInfo);
        }

        RemoveAuthenticatedUser();
        NotifyAuthenticationStateChanged(_authenticationStateTask);
    }

    private async Task SetAuthenticatedUser()
    {
        var user =await storageService.GetItemAsync<LoggedUser>(UserConst.UserInfo);

        if (user == null)
            RemoveAuthenticatedUser();
        else
            SetAuthenticatedUser(user);
    }

    private void SetAuthenticatedUser(LoggedUser user) => _authenticationStateTask = Task.FromResult(new AuthenticationState(
        new ClaimsPrincipal(new ClaimsIdentity(
            user.GetClaims(),
            UserConst.AuthType))));

    private void RemoveAuthenticatedUser() => _authenticationStateTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
}