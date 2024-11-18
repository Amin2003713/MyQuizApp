using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MyQuizApp.Infra.Users;

namespace MyQuiz.App.Web.Services;


public class ClientStateProvider(
    HttpClient httpClient,
    IAuthClient authClient ,
    NavigationManager navigationManager)
    : AuthenticationStateProvider
{

private readonly Task<AuthenticationState> _authenticationStateTask  = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));




    public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
    {
        var state = await GetAuthenticationStateAsync();
        var authenticationStateProviderUser = state.User;
        return authenticationStateProviderUser;
    }


    public override  Task<AuthenticationState> GetAuthenticationStateAsync()
        => _authenticationStateTask;


    public  async Task LoginAsync(LoginRequests requests) =>  await authClient.Login(requests);

}