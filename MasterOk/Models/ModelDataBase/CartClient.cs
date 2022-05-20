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
        [Range(typeof(decimal), "0,0", "100000,6", ErrorMessage = "Стоимость отрицательная")]
        public decimal PriceCartProduct { get; set; }

        [Display(Name = "Количество")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество отрицательное")]
        public int CountCartProduct { get; set; }

        [Display(Name = "Сумма")]
        [Range(typeof(decimal), "0,0", "100000,6", ErrorMessage = "Стоимость отрицательная")]
        public decimal TotalCartProduct { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        [Display(Name = "Клиент")]
        public Client? Client { get; set; }
    }
}
