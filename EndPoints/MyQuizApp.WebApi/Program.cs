using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyQuizApp.Domain.Users;
using MyQuizApp.WebApi.Controllers;
using MyQuizApp.WebApi.Data;
using MyQuizApp.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoryService>();



builder.Services.AddDbContext<Context>(a =>
{
    a.UseSqlServer(builder.Configuration.GetConnectionString("Server"));
});


var secret = "A0725013-F07F-4ED9-B411-383283210A40";
var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

builder.Services.AddAuthentication(schemes =>
    {
        schemes.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        schemes.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        schemes.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    }).
    AddJwtBearer(options =>
    {
        options.Audience = "MyQuizApp.WebApi";
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidAudience = "MyQuizApp.Client" ,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
    });


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.ConfigureAutomaticMigrations();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapAuthApis();
app.MapCategoryEndpoints();

app.Run();


