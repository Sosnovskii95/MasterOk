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
        [Range(0, int.MaxValue, ErrorMessage = "Количество отрицательное")]
        public int CountCartProduct { get; set; }

        [Display(Name = "Сумма")]
        [Range(0, double.MaxValue, ErrorMessage = "Сумма отрицательная")]
        public double TotalCartProduct { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        [Display(Name = "Клиент")]
        public Client? Client { get; set; }
    }
}
