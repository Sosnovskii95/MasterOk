using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Role
    {
        [Key]
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Права доступа")]
        [Required(ErrorMessage = "Права доступа")]
        public string TitleRole { get; set; }

        [Display(Name = "Значение прав доступа (user, admin)")]
        [Required(ErrorMessage = "Значение прав доступа")]
        public string ValueRole { get; set; }
    }
}
