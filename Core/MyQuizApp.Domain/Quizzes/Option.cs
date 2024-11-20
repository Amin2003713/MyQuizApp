using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyQuizApp.Domain.Quizzes;

public class Option
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required(ErrorMessage = "متن گزینه نمی‌تواند خالی باشد.")]
    [StringLength(300, ErrorMessage = "متن گزینه نمی‌تواند بیش از ۳۰۰ کاراکتر باشد.")]
    public string Text { get; set; }

    public bool IsCorrectAnswer { get; set; }

    [Required(ErrorMessage = "شناسه سوال نمی‌تواند خالی باشد.")]
    public Guid QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))] public Question Question { get; set; }
}