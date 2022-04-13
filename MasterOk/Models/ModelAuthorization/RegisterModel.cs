using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelAuthorization
{
    public class RegisterModel
    {
        [Display(Name = "Электронная почта")]
        public string EmailClient { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string PasswordClient { get; set; }

        [Display(Name = "Повторите пароль")]
        [Compare("PasswordClient", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Фамилия Имя Отчество")]
        public string FirstLastNameClient { get; set; }

        [Display(Name = "Номер телефона")]
        public string NumberPhone { get; set; }

        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }
    }
}
