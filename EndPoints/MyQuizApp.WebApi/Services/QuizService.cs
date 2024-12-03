using Microsoft.EntityFrameworkCore;
using MyQuizApp.Domain.Quizzes;
using MyQuizApp.WebApi.Data;

namespace MyQuizApp.WebApi.Services;

using Infra.Quiezzes;

public class QuizService(Context context)
{
    public async Task<ApiResponse> CreateQuizAsync(Quiz quiz)
    {
        try
        {
            context.Set<Quiz>().Add(quiz);
            await context.SaveChangesAsync();
            return ApiResponse.Success("");
        }
        catch (Exception ex)
        {
            return ApiResponse.Failed($"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponseWithData<Quiz>> GetQuizByIdAsync(Guid id)
    {
        var quiz = await context.Set<Quiz>()
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .Include(q => q.Category)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null)
            return ApiResponseWithData<Quiz>.Failed(null, "Quiz not found.");

        return ApiResponseWithData<Quiz>.Success(quiz);
    }
    
    public async Task<ApiResponseWithData<QuizDto>> GetQuizForAttendingByIdAsync(Guid id)
    {
        var quiz = await context.Set<Quiz>()
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .Include(q => q.Category)
            .Select(a=> new QuizDto
            {
                Duration = a.Duration,
                Id = a.Id,
                Name = a.Name,
                CategoryId = a.CategoryId,
                QuestionCount = a.Questions.Count,
                CategoryName = a.Category.Name,
                Questions = a.Questions.Select(w=> new QuestionDto
                {
                         Id = w.Id,
                         Text = w.Text,
                         Options =  w.Options.Select(e=> new OptionDto
                         {
                             Id = e.Id,
                             Text = e.Text,
                         }).ToList(),
                }).ToList(),
            })
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null)
            return ApiResponseWithData<QuizDto>.Failed(null, "Quiz not found.");

        return ApiResponseWithData<QuizDto>.Success(quiz);
    }

    public async Task<ApiResponseWithData<List<Quiz>>> GetAllQuizzesAsync()
    {
        var quizzes = await context.Set<Quiz>().
                                    Include(q => q.Category) // Include Category navigation property
                                    .
                                    Include(q => q.Questions) // Include Questions collection
                                    .
                                    ThenInclude(q => q.Options) // Include Options collection within Questions
                                    .
                                    ToListAsync();


        return ApiResponseWithData<List<Quiz>>.Success(quizzes);
    }


    public async Task<ApiResponseWithData<int>> AttendQuiz(QuizAttendanceCommand quizAttendanceCommand)
    {
        return ApiResponseWithData<int>.Success(12);
    }


    public async Task<ApiResponseWithData<List<QuizDto>>> GetAllActiveQuizzesAsync()
    {
        var quizzes = await context.Set<Quiz>()
                                   .Where(a => a.IsActive)
                                   .Include(q => q.Category) // Include Category navigation property
                                    .Include(q => q.Questions) // Include Questions collection
                                    .ThenInclude(q => q.Options) // Include Options collection within Questions
                                    .Select(a=> new QuizDto
                                    {
                                        Duration = a.Duration,
                                        Id = a.Id,
                                        Name = a.Name,
                                        CategoryId = a.CategoryId,
                                        QuestionCount = a.Questions.Count,
                                    })
                                    .ToListAsync();


        return ApiResponseWithData<List<QuizDto>>.Success(quizzes);
    }
    public async Task<ApiResponse> UpdateQuizAsync(Quiz quiz)
    {
        try
        {
            context.Set<Quiz>().Update(quiz);
            await context.SaveChangesAsync();
            return ApiResponse.Success("Quiz updated successfully.");
        }
        catch (Exception ex)
        {
            return ApiResponse.Failed($"Error: {ex.Message}");
        }
    }

    public async Task<ApiResponse> DeleteQuizAsync(Guid id)
    {
        var quiz = await context.Set<Quiz>().FindAsync(id);
        if (quiz == null)
            return ApiResponse.Failed("Quiz not found.");

        context.Set<Quiz>().Remove(quiz);
        await context.SaveChangesAsync();
        return ApiResponse.Success("Quiz deleted successfully.");
    }
}