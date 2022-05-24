using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class User
    {
        [Key]
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Электронная почта")]
        public string EmailUser { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Логин")]
        public string LoginUser { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Пароль")]
        public string PasswordUser { get; set; }

        [Display(Name = "ФИО сотрудника")]
        [Required(ErrorMessage = "ФИО сотрудника")]
        public string FirstLastNameStaff { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Номер телефона")]
        public int NumberPhoneStaff { get; set; }

        [Display(Name = "Активность")]
        [Required(ErrorMessage = "Активность")]
        public bool ActiveUser { get; set; }

        [Display(Name = "Права доступа")]
        public int RoleId { get; set; }

        [Display(Name = "Права доступа")]
        public Role? Role { get; set; }
    }
}
