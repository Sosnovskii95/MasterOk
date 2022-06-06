using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelAuthorization
{
    public class LoginModel
    {
        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Электронная почта")]
        [EmailAddress(ErrorMessage = "Некорректная электронная почта")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Администратор")]
        public bool InvateAdmin { get; set; }
    }
}
