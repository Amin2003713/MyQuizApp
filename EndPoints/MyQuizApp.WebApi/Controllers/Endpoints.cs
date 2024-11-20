using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyQuizApp.Domain.Users;
using MyQuizApp.Infra.Users;
using MyQuizApp.Infra.Users.Requests;
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

        public static void MapCategoryEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/categories").RequireAuthorization();

            // Get all categories
            group.MapGet("/", async (CategoryService service, CancellationToken cancellationToken) =>
            {
                var response = await service.GetAllCategoriesAsync(cancellationToken);
                return Results.Ok(response);
            });

            // Get a category by ID
            group.MapGet("/{id:guid}", async (Guid id, CategoryService service) =>
            {
                var response = await service.GetCategoryByIdAsync(id);
                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            });

            // Create a new category
            group.MapPost("/", async ([FromBody] CreateCategoryDto createDto, CategoryService service) =>
            {
                var response = await service.CreateCategoryAsync(createDto);
                return response.IsSuccess
                    ? Results.Created($"/api/categories/{response.Data.Id}", response)
                    : Results.BadRequest(response);
            }).
            RequireAuthorization(r => r.RequireRole(nameof(UserRoles.Admin)));

            // Update an existing category
            group.MapPut("/{id:guid}", async (Guid id,[FromBody] UpdateCategoryDto updateDto, CategoryService service) =>
            {
                var response = await service.UpdateCategoryAsync(id, updateDto);
                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            }).
            RequireAuthorization(r => r.RequireRole(nameof(UserRoles.Admin)));

            // Delete a category
            group.MapDelete("/{id:guid}", async (Guid id, CategoryService service) =>
            {
                var response = await service.DeleteCategoryAsync(id);
                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            }).
            RequireAuthorization(r => r.RequireRole(nameof(UserRoles.Admin)));
        }


}