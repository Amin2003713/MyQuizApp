using MyQuizApp.Infra.Services;

namespace MyQuizApp.App.Services;

public class FormFactor : IFormFactor
{
    public string GetFormFactor()
    {
        return "Environment";
    }

    public string GetPlatform()
    {
        return Environment.OSVersion.ToString();
    }
}