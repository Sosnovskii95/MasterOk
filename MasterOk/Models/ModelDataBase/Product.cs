using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Product
    {
        [Key]
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Название товара")]
        [Required(ErrorMessage = "Название товара")]
        public string TitleProduct { get; set; }

        [Display(Name = "Описание товара")]
        [Required(ErrorMessage = "Описание товара")]
        [MaxLength(5000)]
        public string DescriptionProduct { get; set; }

        [Display(Name = "Гарантия")]
        [Required(ErrorMessage = "Гарантия")]
        [Range(0, int.MaxValue, ErrorMessage = "Гарантия отрицательная")]
        public int Warranty { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Стоимость")]
        [Range(typeof(decimal), "0,0", "100000,9", ErrorMessage = "Стоимость отрицательная")]
        public decimal Price { get; set; }

        [Display(Name = "Подкатегория")]
        public int? SubCategoryId { get; set; }

        [Display(Name = "Подкатегория")]
        public SubCategory? SubCategory { get; set; }

        public ICollection<PathImage>? NameImages { get; set; }

        [Display(Name = "Количество на складе")]
        [Required(ErrorMessage = "Количество на складе")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество отрицательное")]
        public int CountStoreProduct { get; set; }
    }
}
