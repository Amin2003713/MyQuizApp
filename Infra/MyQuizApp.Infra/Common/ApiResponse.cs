namespace MyQuizApp.WebApi.Services;

public record ApiResponse(bool IsSuccess , string Error) : IApiResponse
{
    public static ApiResponse Success(string error) => new(true, error);
    public static ApiResponse Failed(string error) => new(false, error);
}