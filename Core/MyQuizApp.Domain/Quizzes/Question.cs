using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyQuizApp.Domain.Quizzes;

public class Question
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required(ErrorMessage = "متن سوال نمی‌تواند خالی باشد.")]
    [StringLength(500, ErrorMessage = "متن سوال نمی‌تواند بیش از ۵۰۰ کاراکتر باشد.")]
    public string Text { get; set; }

    [Required(ErrorMessage = "شناسه آزمون نمی‌تواند خالی باشد.")]
    public Guid QuizId { get; set; }

    [ForeignKey(nameof(QuizId))] public Quiz Quiz { get; set; }

    public ICollection<Option> Options { get; set; } = new List<Option>();
}