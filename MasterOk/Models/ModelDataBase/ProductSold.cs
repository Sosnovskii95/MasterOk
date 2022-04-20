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

        public Product Product { get; set; }

        public int CountSold { get; set; }

        public double PriceSold { get; set; }

        public double TotalSold { get; set; }
    }
}
