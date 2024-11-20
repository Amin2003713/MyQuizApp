using System.ComponentModel.DataAnnotations;

namespace MyQuizApp.Infra.Users.Requests;

public class LoginRequests
{
        [Required(ErrorMessage = "ایمیل نمی‌تواند خالی باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز عبور نمی‌تواند خالی باشد.")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد.")]
        public string Password { get; set; }
}
