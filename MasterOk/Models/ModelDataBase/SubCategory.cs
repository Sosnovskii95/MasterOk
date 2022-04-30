using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название подкатегории")]
        public string TitleSubCategory { get; set; }

        [Display(Name = "Изображение")]
        public ICollection<PathImage>? NameImages { get; set; }

        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        [Display(Name = "Категория")]
        public Category? Category { get; set; }

        [Display(Name = "Товары")]
        public ICollection<Product>? Products { get; set; }
    }
}
