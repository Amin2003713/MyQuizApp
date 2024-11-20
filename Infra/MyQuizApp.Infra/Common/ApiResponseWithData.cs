namespace MyQuizApp.WebApi.Services;

public record ApiResponseWithData<TData>(TData Data , bool IsSuccess, string Error) : IApiResponse
{
    public static ApiResponseWithData<TData> Success(TData data) => new(data, true, "Success") ;
    public static ApiResponseWithData<TData> Failed(TData data , string error) => new(data, true, error);
            
}