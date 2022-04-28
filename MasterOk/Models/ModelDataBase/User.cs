using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Электронная почта пользователя")]
        public string EmailUser { get; set; }

        [Display(Name = "Логин пользователя")]
        public string LoginUser { get; set; }

        [Display(Name = "Пароль пользователя")]
        public string PasswordUser { get; set; }

        [Display(Name = "Сотрудник")]
        public int StaffId { get; set; }

        [Display(Name = "Сотрудник")]
        public Staff? Staff { get; set; }

        [Display(Name = "Роль пользователя")]
        public int RoleId { get; set; }

        [Display(Name = "Роль пользователя")]
        public Role? Role { get; set; }
    }
}
