using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Электронная почта")]
        [DataType(DataType.EmailAddress)]
        public string EmailClient { get; set; }

        [Display(Name = "Пароль")]
        public string PasswordClient { get; set; }

        [Display(Name = "Фамилия Имя Отчество")]
        public string FirstLastNameClient { get; set; }

        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string NumberPhone { get; set; }

        [Display(Name ="Адрес доставки")]
        public string Address { get; set; }

        [Display(Name ="Процент скидки")]
        public int ProcentSalary { get; set; }

        public ICollection<ProductCheck>? ProductChecks { get; set; }

        public ICollection<CartClient>? CartClients { get; set; }
    }
}
