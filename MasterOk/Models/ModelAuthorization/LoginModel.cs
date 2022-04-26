using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelAuthorization
{
    public class LoginModel
    {
        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string Password { get; set; }
    }
}
