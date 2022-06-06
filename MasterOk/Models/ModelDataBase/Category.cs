using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название категории")]
        public string TitleCategory { get; set; }

        [Display(Name = "Изображение")]
        public ICollection<PathImage>? NameImages { get; set; }

        [Display(Name = "Список подкатегорий")]
        public ICollection<SubCategory>? SubCategories { get; set; }
    }
}
