using MyQuizApp.Infra.Users.Requests;
using MyQuizApp.Infra.Users.Response;
using MyQuizApp.Infra.Users.Routs;
using Refit;

namespace MyQuizApp.Infra.Users;

public interface IUserApiClient
{
    [Post(AuthRouts.Login)]
    Task<LoginResponse> Login(LoginRequests requests);
}

public static class UserConst
{
    public static string UserInfo { get; set; } = "UserInfo";
    public static string AuthType { get; set; } = "QuizApplication";
}




