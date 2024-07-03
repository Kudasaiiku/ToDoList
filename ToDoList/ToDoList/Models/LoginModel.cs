using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        [StringLength(200, ErrorMessage = "Email не должен превышать 200 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(200, MinimumLength = 8, ErrorMessage = "Пароль должен содержать минимум 8 символов и не должен превышать 200 символов")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Пароль не должен содержать пробелы")]
        public string Password { get; set; }
    }
}
