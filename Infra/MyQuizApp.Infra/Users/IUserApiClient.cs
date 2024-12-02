using MyQuizApp.Infra.Users.Requests;
using MyQuizApp.Infra.Users.Response;
using MyQuizApp.Infra.Users.Routs;
using Refit;

namespace MyQuizApp.Infra.Users;

using Domain.Users;
using WebApi.Services;

public interface IUserApiClient
{
    [Post(AuthRouts.Login)]
    Task<LoginResponse> Login(LoginRequests requests);
    
    [Post(AuthRouts.StudentRegister)]
    Task<ApiResponse> StudentRegister(StudentRegisterModel requests);

    [Get(AuthRouts.ListUsers)]
    Task<ApiResponseWithData<List<User>>> ListUsers();

    [Put("/users/activate/{userId}")]
    Task<ApiResponse> StudentActivate(Guid userId);
}

public static class UserConst
{
    public static string UserInfo { get; set; } = "UserInfo";
    public static string AuthType { get; set; } = "QuizApplication";
}




