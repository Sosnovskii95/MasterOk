using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Электронная почта пользователя")]
        [DataType(DataType.EmailAddress)]
        public string EmailUser { get; set; }

        [Display(Name = "Логин пользователя")]
        public string LoginUser { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string PasswordUser { get; set; }

        [Display(Name = "ФИО сотрудника")]
        public string FirstLastNameStaff { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Номер телефона")]
        public int NumberPhoneStaff { get; set; }

        [Display(Name ="Активность пользователя")]
        public bool ActiveUser { get; set; }

        [Display(Name = "Роль пользователя")]
        public int RoleId { get; set; }

        [Display(Name = "Роль пользователя")]
        public Role? Role { get; set; }
    }
}
