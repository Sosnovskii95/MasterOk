using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelAuthorization
{
    public class RegisterModel
    {
        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Электронная почта")]
        [EmailAddress(ErrorMessage = "Некорректная электронная почта")]
        public string EmailClient { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Пароль")]
        public string PasswordClient { get; set; }

        [Display(Name = "Повторите пароль")]
        [Compare("PasswordClient", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Повторите пароль")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Фамилия Имя Отчество")]
        [Required(ErrorMessage = "Фамилия Имя Отчество")]
        public string FirstLastNameClient { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Номер телефона")]
        public string NumberPhone { get; set; }

        [Display(Name = "Адрес доставки")]
        [Required(ErrorMessage = "Адрес доставки")]
        public string Address { get; set; }
    }
}
