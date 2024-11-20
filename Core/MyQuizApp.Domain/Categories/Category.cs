using System.ComponentModel.DataAnnotations;

namespace MyQuizApp.Domain.Categories;

public class Category
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required(ErrorMessage = "نام دسته‌بندی نمی‌تواند خالی باشد.")]
    [StringLength(100, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیش از ۱۰۰ کاراکتر باشد.")]
    public string Name { get; set; }
}