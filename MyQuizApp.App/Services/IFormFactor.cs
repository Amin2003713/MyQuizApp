using MyQuizApp.Infra.Services;

namespace MyQuizApp.App.Services;

using Infra.Common;

public class FormFactor : IFormFactor
{
    public ApplicationTypes GetFormFactor()
    {
        return ApplicationTypes.Maui;
    }

    public string GetPlatform()
    {
        return Environment.OSVersion.ToString();
    }
}