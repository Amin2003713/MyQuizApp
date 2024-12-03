using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyQuizApp.Domain.Quizzes;
using MyQuizApp.Domain.Users;
using MyQuizApp.Infra.Users;
using MyQuizApp.Infra.Users.Requests;
using MyQuizApp.Infra.Users.Routs;
using MyQuizApp.WebApi.Services;

namespace MyQuizApp.WebApi.Controllers;

using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class Endpoints {

    public static IEndpointRouteBuilder MapAuthApis(this IEndpointRouteBuilder endpoints)
    {

        endpoints.MapPost(AuthRouts.Login, async (LoginRequests request, AuthService service  ,  CancellationToken token) => Results.Ok((object?)await
            service.Authenticate(request, token)));

        endpoints.MapPost(
            "/api/users/StudentRegister"
            , async (StudentRegisterModel user, IPasswordHasher<User> hasher, Context dbContext) =>
              {
                  // Check if the email already exists
                  var existingUser =
                      await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.Phone == user.Phone);
                  if (existingUser != null)
                  {
                      return Results.BadRequest(ApiResponse.Failed("Email or phone already exists"));
                  }

                  var user1 = new User()
                  {
                      Email = user.Email, UserRoles = UserRoles.Student, Phone = user.Phone, Name = user.Name
                      , IsActive = false,
                  };
                  // Hash the password (you should use a secure method like bcrypt in production)
                  user1.PasswordHash = hasher.HashPassword(user1, user.PasswordHash);

                  // Add the user to the database
                  dbContext.Users.Add(user1);
                  await dbContext.SaveChangesAsync();

                  return Results.Ok(ApiResponse.Success("User registered"));
              });

        endpoints.MapPut(
            "/users/activate/{id:guid}"
            , async (Guid id, Context dbContext) =>
              {
                  var user = await dbContext.Users.FindAsync(id);

                  if (user == null)
                  {
                      return Results.NotFound("User not found");
                  }

                  user.IsActive = !user.IsActive;

                  await dbContext.SaveChangesAsync();

                  return Results.Ok(ApiResponse.Success("done"));
              });


        endpoints.MapGet(
            "/Admin/Users/ListUsers"
            , async (Context dbContext) =>
              {
                  var users = await dbContext.Users.Where(a=>a.UserRoles != UserRoles.Admin).ToListAsync();
                  return users.Count > 0 ?  Results.Ok(ApiResponseWithData<List<User>>.Success(users)) : Results.NotFound();
              });


        return endpoints;
    }

    public static void MapQuizzesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/api/quizzes").RequireAuthorization();

        group.MapPost("/", async (Quiz quiz, QuizService quizService) =>
        {
            var result = await quizService.CreateQuizAsync(quiz);
            return result.IsSuccess
                ? Results.Ok(ApiResponse.Success("WellDone"))
                : Results.BadRequest(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, QuizService quizService) =>
        {
            var result = await quizService.GetQuizByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
        });
        
        group.MapGet("/GetForAttender/{id:guid}", async (Guid id, QuizService quizService) =>
        {
            var result = await quizService.GetQuizForAttendingByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
        });

        group.MapGet("/ListAll", async (QuizService quizService) =>
        {
            var result = await quizService.GetAllQuizzesAsync();
            return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
        });
        
        group.MapGet("/ListActiveAll", async (QuizService quizService) =>
        {
            var result = await quizService.GetAllActiveQuizzesAsync();
            return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
        });

        group.MapPut("/{id:guid}", async (Guid id, Quiz quiz, QuizService quizService) =>
        {
            if (id != quiz.Id)
                return Results.BadRequest(ApiResponse.Failed("ID mismatch."));

            var result = await quizService.UpdateQuizAsync(quiz);
            return result.IsSuccess ? Results.Ok(ApiResponse.Success("WellDone")) : Results.BadRequest(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, QuizService quizService) =>
        {
            var result = await quizService.DeleteQuizAsync(id);
            return result.IsSuccess ? Results.Ok(ApiResponse.Success("WellDone")) : Results.BadRequest(result);
        });
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