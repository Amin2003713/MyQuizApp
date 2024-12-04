using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyQuizApp.Infra.Categories;
using MyQuizApp.Infra.Users;
using Refit;

namespace MyQuizApp.Infra.Services;

using Common;

public interface IFormFactor
{
    public ApplicationTypes GetFormFactor();
    public string GetPlatform();
}





