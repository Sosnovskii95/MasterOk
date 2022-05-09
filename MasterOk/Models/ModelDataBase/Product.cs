using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Название товара")]
        public string TitleProduct { get; set; }

        [Display(Name = "Описание товара")]
        public string DescriptionProduct { get; set; }

        [Display(Name = "Гарантия")]
        [Range(0, int.MaxValue, ErrorMessage = "Гарантия отрицательная")]
        public int Warranty { get; set; }

        [Display(Name = "Стоимость")]
        [Range(0, double.MaxValue, ErrorMessage = "Стоимость отрицательная")]
        public double Price { get; set; }

        [Display(Name = "Подкатегория")]
        public int? SubCategoryId { get; set; }

        [Display(Name = "Подкатегория")]
        public SubCategory? SubCategory { get; set; }

        public ICollection<PathImage>? NameImages { get; set; }

        [Display(Name = "Количество на складе")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество отрицательное")]
        public int CountStoreProduct { get; set; }
    }
}
