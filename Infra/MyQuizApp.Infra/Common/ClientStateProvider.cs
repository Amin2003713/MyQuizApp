using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyQuizApp.Infra.Users;
using MyQuizApp.Infra.Users.Requests;
using MyQuizApp.Infra.Users.Response;

namespace MyQuizApp.Infra.Common;

using Domain.Users;
using Microsoft.AspNetCore.Components;
using Services;

public class ClientStateProvider(
    ILocalStorage storageService,
    IUserApiClient userApiClient ,
    NavigationManager navigationManager,
    IAppState appState,
    IFormFactor formFactor)
    : AuthenticationStateProvider
{

    private Task<AuthenticationState> _authenticationStateTask  = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));


    public LoggedUser? LoggedUser { get; private set; }
    public bool IsLoggedIn => LoggedUser != null;
    private static readonly SemaphoreSlim Semaphore = new(1, 1);


    public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
    {
        var state                           = await GetAuthenticationStateAsync();
        var authenticationStateProviderUser = state.User;
        return authenticationStateProviderUser;
    }


    public override  Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask;


    public  async Task<LoginResponse> LoginAsync(LoginRequests requests)
    {
        try
        {
            appState.ShowLoader();

            var result = await userApiClient.Login(requests);

            if (result.HasError)
                return result;

            var a = OperatingSystem.IsBrowser(); 
            var aa = OperatingSystem.IsAndroid(); 
            var aas = OperatingSystem.IsWindows(); 

            if (OperatingSystem.IsBrowser() && result.Token.UserRoles == UserRoles.Admin)
                return new LoginResponse(null!, "Admin connat use the application", true);

            await storageService.SetItemAsync(UserConst.UserInfo, result.Token);
            LoggedUser = result.Token;


            SetAuthenticatedUser(result.Token);
            NotifyAuthenticationStateChanged(_authenticationStateTask);

            appState.HideLoader();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            appState.HideLoader();
            return default!;
        }
    }

    public async Task LogoutAsync()
    {
        appState.ShowLoader();

        if(IsLoggedIn)
        {
            LoggedUser = null;
            await storageService.RemoveItemAsync(UserConst.UserInfo);
        }

        RemoveAuthenticatedUser();
        NotifyAuthenticationStateChanged(_authenticationStateTask);
        appState.HideLoader();

    }

    public async Task SetAuthenticatedUser()
    {

            appState.ShowLoader();
            await Semaphore.WaitAsync();
            try
            {
                var user = await storageService.GetItemAsync<LoggedUser>(UserConst.UserInfo);

                if (user == null)
                {
                    RemoveAuthenticatedUser();
                    if(formFactor.GetFormFactor() == ApplicationTypes.Maui)
                        return;

                    navigationManager.NavigateTo("/login");
                }
                else
                {
                    LoggedUser = user;
                    SetAuthenticatedUser(user);

                    if (formFactor.GetFormFactor() == ApplicationTypes.Maui)
                        return;

                    navigationManager.NavigateTo(user.UserRoles is UserRoles.Student ? "/Student/Home" : "/");
                }
            }
            finally
            {
                NotifyAuthenticationStateChanged(_authenticationStateTask);
                Semaphore.Release(); // Release the lock
                appState.HideLoader();
            }
    }


    private void SetAuthenticatedUser(LoggedUser user) => _authenticationStateTask = Task.FromResult(new AuthenticationState(
        new ClaimsPrincipal(new ClaimsIdentity(
            user.GetClaims(),
            UserConst.AuthType))));

    private void RemoveAuthenticatedUser() => _authenticationStateTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));


}