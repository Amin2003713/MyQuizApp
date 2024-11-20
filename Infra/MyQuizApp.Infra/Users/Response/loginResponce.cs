using System.Security.Claims;
using MyQuizApp.Domain.Users;

namespace MyQuizApp.Infra.Users.Response;

public record LoginResponse(LoggedUser Token, string? Error, bool HasError);

public class LoggedUser : User
{
    public string? Token { get; set; }


    public Claim[] GetClaims() =>
    [
        new(ClaimTypes.NameIdentifier, Id.ToString()),
        new(ClaimTypes.Email, Email),
        new(ClaimTypes.Name, Name),
        new(ClaimTypes.MobilePhone, Phone),
        new(ClaimTypes.Role, UserRoles.ToString())
    ];
}