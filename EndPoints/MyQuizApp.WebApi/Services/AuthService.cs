using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyQuizApp.Domain.Users;
using MyQuizApp.Infra.Users;
using MyQuizApp.Infra.Users.Requests;
using MyQuizApp.Infra.Users.Response;
using MyQuizApp.WebApi.Data;

namespace MyQuizApp.WebApi.Services;

public class AuthService(IPasswordHasher<User> passwordHasher, Context db)
{
    public async Task<LoginResponse?> Authenticate(LoginRequests loginRequest, CancellationToken cancelationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email, cancelationToken);

        if (user is null)
            return new LoginResponse( null , "Not Found" , true);

        if(user.IsActive is false )
            return new LoginResponse(null, "unauthrozied Access ples Active ur acocunt ", true);
             

        var passResult = passwordHasher.VerifyHashedPassword(user , user.PasswordHash ,  loginRequest.Password);

        if(passResult == PasswordVerificationResult.Failed)
            return new LoginResponse(null, "Password is not correct", true);

        var token = CreateToken(user);

        return new LoginResponse(new LoggedUser()
        {
            Email = user.Email,
            UserRoles = user.UserRoles , 
                               Name = user.Name,
                               Id = user.Id,
                               Phone = user.Phone,
                               PasswordHash = user.PasswordHash,
                               Token = token,
            IsActive = user.IsActive
        }, null, false);
    }



    private static string CreateToken(User user)
    {
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Role, user.UserRoles.ToString()),
        ];
        var secret = "A0725013-F07F-4ED9-B411-383283210A40";
        var key =new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
         var jwtToken = new JwtSecurityToken(
            issuer: "MyQuizApp.WebApi",
             audience: "MyQuizApp.Client",
             claims: claims,
            expires: DateTime.Now.AddMinutes(3600),
             signingCredentials: creds
         );
         return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}