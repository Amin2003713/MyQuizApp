using MyQuizApp.Domain.Quizzes;
using MyQuizApp.WebApi.Services;
using Refit;

namespace MyQuizApp.Infra.Quiezzes;

[Headers("authorization: Bearer ")]
public interface IQuizApi
{
    [Post("/api/quizzes")]
    Task<ApiResponseWithData<Quiz>> CreateQuizAsync([Body] Quiz quiz);

    [Get("/api/quizzes/{id}")]
    Task<ApiResponseWithData<Quiz>> GetQuizByIdAsync(Guid id);

    [Get("/api/quizzes/ListAll")]
    Task<ApiResponseWithData<IEnumerable<Quiz>>> GetAllQuizzesAsync();

    [Put("/api/quizzes/{id}")]
    Task<ApiResponse> UpdateQuizAsync(Guid id, [Body] Quiz quiz);

    [Delete("/api/quizzes/{id}")]
    Task<ApiResponse> DeleteQuizAsync(Guid id);
}


public static class QuizConst
{
    public static string QuestionsStepper { get; set; } = "Question";
}