namespace MyQuizApp.WebApi.Services;

public interface IApiResponse
{
    public bool IsSuccess { get; init; }
    public string Error { get; init; }
}