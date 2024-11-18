using MyQuizApp.Infra.Users.Response;
using MyQuizApp.Infra.Users.Routs;
using Refit;

namespace MyQuizApp.Infra.Users;

public interface IAuthClient
{
    [Post(AuthRouts.Login)]
    Task<LoginResponse> Login(LoginRequests requests);
}


