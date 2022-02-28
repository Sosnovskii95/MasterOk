using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelAuthorization
{
    public class RegisterModel
    {
        [Display(Name = "Логин")]
        [Required]
        public string LoginClient { get; set; }

        [Display(Name = "Электронная почта")]
        public string EmailClient { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string PasswordClient { get; set; }

        [Display(Name = "Повторите пароль")]
        [Compare("PasswordClient", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Фамилия")]
        public string FirstNameClient { get; set; }

        [Display(Name = "Имя")]
        public string LastNameClient { get; set; }
    }
}
