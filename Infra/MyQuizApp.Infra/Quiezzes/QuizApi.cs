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

    [Get("/api/quizzes/GetForAttender/{id}")]
    Task<ApiResponseWithData<QuizDto>> GetQuizForAttendingByIdAsync(Guid id);

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
    public string CategoryName { get; set; }

    public List<QuestionDto> Questions { get; set; }
}

public class QuestionDto
{
    public string Text { get; set; }

    public List<OptionDto> Options { get; set; }
    public Guid Id { get; set; }
}

public class OptionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
}

public class QuizAttendanceCommand
{
public Guid QuizId { get; set; }

public DateTime StartTime { get; set; }

public DateTime EndTime { get; set; }

public TimeSpan DurationTook { get; set; }

public List<QuestionCommand> Questions { get; set; } = [];

}

public class QuestionCommand
{
public Guid QuestionId { get; set; }

public Guid OptionSelected { get; set; }

public DateTime AtDatetime { get; set; }
}

