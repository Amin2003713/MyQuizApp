using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyQuizApp.Infra.Categories;
using MyQuizApp.Infra.Users;
using Refit;

namespace MyQuizApp.Infra.Services;

public interface IFormFactor
{
    public string GetFormFactor();
    public string GetPlatform();
}





