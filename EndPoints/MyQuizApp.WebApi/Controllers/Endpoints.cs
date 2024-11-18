using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyQuizApp.Infra.Users;
using MyQuizApp.Infra.Users.Routs;
using MyQuizApp.WebApi.Services;

namespace MyQuizApp.WebApi.Controllers;


public static class Endpoints {

    public static IEndpointRouteBuilder MapAuthApis(this IEndpointRouteBuilder endpoints)
    {

        endpoints.MapPost(AuthRouts.Login, async (LoginRequests request, AuthService service , CancellationToken token) => Results.Ok((object?)await
            service.Authenticate(request, token)));



        return endpoints;
    }
}