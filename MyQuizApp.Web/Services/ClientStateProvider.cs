using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyQuizApp.Infra.Users;
using MyQuizApp.Infra.Users.Requests;
using MyQuizApp.Infra.Users.Response;

namespace MyQuizApp.Web.Services;

using Domain.Users;
using Infra.Users.Routs;
using Microsoft.AspNetCore.Components;

public class ClientStateProvider(
    ILocalStorageService storageService,
    IUserApiClient userApiClient , NavigationManager navigationManager)
    : AuthenticationStateProvider
{

private Task<AuthenticationState> _authenticationStateTask  = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));


public LoggedUser? LoggedUser { get; private set; }
public bool IsLoggedIn => LoggedUser != null;
public bool IsInitialized = false;
private static readonly SemaphoreSlim Semaphore = new(1, 1);


    public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
    {
        var state = await GetAuthenticationStateAsync();
        var authenticationStateProviderUser = state.User;
        return authenticationStateProviderUser;
    }


    public override  Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask;


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

    public async Task SetAuthenticatedUser()
    {
        await Semaphore.WaitAsync(); 
        try
        {
            var user = await storageService.GetItemAsync<LoggedUser>(UserConst.UserInfo);

            if (user == null)
            {
                RemoveAuthenticatedUser();
                navigationManager.NavigateTo("/login");
            }
            else
            {
                LoggedUser = user;
                SetAuthenticatedUser(user);
                navigationManager.NavigateTo(user.UserRoles is UserRoles.Student ? "/Student/Home" : "/");
            }
        }
        finally
        {
            IsInitialized = true;
            NotifyAuthenticationStateChanged(_authenticationStateTask);
            Semaphore.Release(); // Release the lock
        }
    }


    private void SetAuthenticatedUser(LoggedUser user) => _authenticationStateTask = Task.FromResult(new AuthenticationState(
        new ClaimsPrincipal(new ClaimsIdentity(
            user.GetClaims(),
            UserConst.AuthType))));

    private void RemoveAuthenticatedUser() => _authenticationStateTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));


}