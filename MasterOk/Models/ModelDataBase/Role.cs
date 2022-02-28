using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название роли пользователя")]
        public string TitleRole { get; set; }
    }
}
