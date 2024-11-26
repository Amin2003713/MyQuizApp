using Microsoft.EntityFrameworkCore;
using MyQuizApp.Domain.Quizzes;
using MyQuizApp.WebApi.Data;

namespace MyQuizApp.WebApi.Services;

public class QuizService(Context context)
{
    public async Task<ApiResponseWithData<Quiz>> CreateQuizAsync(Quiz quiz)
    {
        try
        {
            context.Set<Quiz>().Add(quiz);
            await context.SaveChangesAsync();
            return ApiResponseWithData<Quiz>.Success(quiz);
        }
        catch (Exception ex)
        {
            return ApiResponseWithData<Quiz>.Failed(null, $"Error: {ex.Message}");
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

    public async Task<ApiResponseWithData<IEnumerable<Quiz>>> GetAllQuizzesAsync()
    {
        var quizzes = await context.Set<Quiz>()
            .Include(q => q.Category)
            .ToListAsync();

        return ApiResponseWithData<IEnumerable<Quiz>>.Success(quizzes);
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