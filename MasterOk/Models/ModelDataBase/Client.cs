using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Электронная почта")]
        public string EmailClient { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Пароль")]
        public string PasswordClient { get; set; }

        [Display(Name = "Фамилия Имя Отчество")]
        [Required(ErrorMessage = "Фамилия Имя Отчество")]
        public string FirstLastNameClient { get; set; }

        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Номер телефона")]
        public string NumberPhone { get; set; }

        [Display(Name = "Адрес доставки")]
        [Required(ErrorMessage = "Адрес доставки")]
        public string Address { get; set; }

        [Display(Name = "Процент скидки")]
        public int ProcentSalaryId { get; set; }

        [Display(Name = "Процент скидки")]
        public ProcentSalary? ProcentSalary { get; set; }

        public ICollection<ProductCheck>? ProductChecks { get; set; }

        public ICollection<CartClient>? CartClients { get; set; }
    }
}
