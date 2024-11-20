using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyQuizApp.Domain.Categories;

namespace MyQuizApp.Domain.Quizzes;

public class Quiz
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required(ErrorMessage = "نام آزمون نمی‌تواند خالی باشد.")]
    [StringLength(200, ErrorMessage = "نام آزمون نمی‌تواند بیش از ۲۰۰ کاراکتر باشد.")]
    public string Name { get; set; }

    [Range(1, 1000, ErrorMessage = "تعداد سوالات باید بین ۱ و ۱۰۰۰ باشد.")]
    public int QuestionCount { get; set; }

    [Required(ErrorMessage = "تاریخ ایجاد نمی‌تواند خالی باشد.")]
    public DateTime CreatedOn { get; set; }

    [Required(ErrorMessage = "مدت‌زمان آزمون نمی‌تواند خالی باشد.")]
    public TimeSpan Duration { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "دسته‌بندی آزمون باید مشخص شود.")]
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))] public virtual Category Category { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}