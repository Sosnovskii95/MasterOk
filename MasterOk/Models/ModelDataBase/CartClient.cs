using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class CartClient
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Товар")]
        public int ProductId { get; set; }

        [Display(Name = "Товар")]
        public Product Product { get; set; }

        [Display(Name = "Цена")]
        public double PriceCartProduct { get; set; }

        [Display(Name = "Количество")]
        public int CountCartProduct { get; set; }

        [Display(Name = "Сумма")]
        public double TotalCartProduct { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        [Display(Name = "Клиент")]
        public Client? Client { get; set; }
    }
}
