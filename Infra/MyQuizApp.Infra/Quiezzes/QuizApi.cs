using MyQuizApp.Domain.Quizzes;
using MyQuizApp.WebApi.Services;
using Refit;

namespace MyQuizApp.Infra.Quiezzes;

[Headers("authorization: Bearer ")]
public interface IQuizApi
{
    [Post("/api/quizzes")]
    Task<ApiResponse> CreateQuizAsync([Body] Quiz quiz);

    [Get("/api/quizzes/{id}")]
    Task<ApiResponseWithData<Quiz>> GetQuizByIdAsync(Guid id);

    [Get("/api/quizzes/ListAll")]
    Task<ApiResponseWithData<IEnumerable<Quiz>>> GetAllQuizzesAsync();

    [Get("/api/quizzes/ListActiveAll")]
    Task<ApiResponseWithData<IEnumerable<QuizDto>>> GetAllActiveQuizzesAsync();

    [Put("/api/quizzes/{id}")]
    Task<ApiResponse> UpdateQuizAsync(Guid id, [Body] Quiz quiz);

    [Delete("/api/quizzes/{id}")]
    Task<ApiResponse> DeleteQuizAsync(Guid id);
}


public static class QuizConst
{
    public static string QuestionsStepper { get; set; } = "Question";
}


public class QuizDto
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; }
    public int QuestionCount { get; set; } = 1;
    public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(20);
    public Guid CategoryId { get; set; }
}