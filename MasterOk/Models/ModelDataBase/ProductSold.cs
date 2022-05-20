using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class ProductSold
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Номер заказа")]
        public int ProductCheckId { get; set; }

        public ProductCheck ProductCheck { get; set; }

        [Display(Name = "Товар")]
        public int ProductId { get; set; }

        [Display(Name = "Товар")]
        public Product Product { get; set; }

        [Display(Name = "Количество")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество отрицательное")]
        public int CountSold { get; set; }

        [Display(Name = "Стоимость")]
        [Range(typeof(decimal), "0,0", "100000,6", ErrorMessage = "Стоимость отрицательная")]
        public decimal PriceSold { get; set; }

        [Display(Name = "Общая стоимость")]
        [Range(typeof(decimal), "0,0", "100000,6", ErrorMessage = "Общая стоимость отрицательная")]
        public decimal TotalSold { get; set; }
    }
}
