using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyQuizApp.Shared.Utiles.Atterbutes;

namespace MyQuizApp.Domain.Users;

public class StudentQuiz
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required(ErrorMessage = "تاریخ شروع نمی‌تواند خالی باشد.")]
    public DateTime StartedAt { get; set; }

    [Required(ErrorMessage = "تاریخ پایان نمی‌تواند خالی باشد.")]
    [DateGreaterThan("StartedAt", ErrorMessage = "تاریخ پایان باید بعد از تاریخ شروع باشد.")]
    public DateTime EndedAt { get; set; }

    [Range(0, 100, ErrorMessage = "نمره باید بین ۰ تا ۱۰۰ باشد.")]
    public int Score { get; set; }

    [Required(ErrorMessage = "آزمون باید مشخص شود.")]
    public Guid QuizId { get; set; }

    [ForeignKey(nameof(QuizId))] public Quiz Quiz { get; set; }

    [Required(ErrorMessage = "دانش‌آموز باید مشخص شود.")]
    public Guid StudentId { get; set; }

    [ForeignKey(nameof(StudentId))] public User Student { get; set; }
}