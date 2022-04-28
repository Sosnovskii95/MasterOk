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
        public int CountSold { get; set; }

        [Display(Name = "Стоимость")]
        public double PriceSold { get; set; }

        [Display(Name = "Общая стоимость")]
        public double TotalSold { get; set; }
    }
}
