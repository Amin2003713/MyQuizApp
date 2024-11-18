using System.ComponentModel.DataAnnotations;

namespace MyQuizApp.Domain.Users;

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required(ErrorMessage = "نام نمی‌تواند خالی باشد.")]
    [StringLength(100, ErrorMessage = "نام نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "ایمیل نمی‌تواند خالی باشد.")]
    [EmailAddress(ErrorMessage = "لطفاً یک ایمیل معتبر وارد کنید.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "شماره تلفن نمی‌تواند خالی باشد.")]
    [Phone(ErrorMessage = "لطفاً یک شماره تلفن معتبر وارد کنید.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "کلمه عبور نمی‌تواند خالی باشد.")]
    public string PasswordHash { get; set; }

    public UserRoles UserRoles { get; set; } = UserRoles.Student;

    public bool IsActive { get; set; } = false;
}