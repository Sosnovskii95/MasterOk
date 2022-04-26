using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelAuthorization
{
    public class RegisterModel
    {
        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string EmailClient { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string PasswordClient { get; set; }

        [Display(Name = "Повторите пароль")]
        [Compare("PasswordClient", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Фамилия Имя Отчество")]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string FirstLastNameClient { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string NumberPhone { get; set; }

        [Display(Name = "Адрес доставки")]
        [Required(ErrorMessage = "Заполните данное поле")]
        public string Address { get; set; }
    }
}
