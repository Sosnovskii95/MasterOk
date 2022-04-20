using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class ProductCheck
    {
        [Key]
        [Display(Name = "Номер заказа")]
        public int Id { get; set; }

        [Display(Name = "Дата заказа")]
        public DateTime DateTimeSale { get; set; }

        [Display(Name = "Статус заказа")]
        public string StateOrder { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        public Client? Client { get; set; }

        [Display(Name = "Менеджер")]
        public int? UserId { get; set; }

        public User? User { get; set; }

        public ICollection<ProductSold> ProductSolds { get; set; }

        public int? PayMethodId { get; set; }

        [Display(Name = "Способ оплаты")]
        public PayMethod? PayMethod { get; set; }

        public int? DeliveryMethodId { get; set; }

        [Display(Name = "Способ доставки")]
        public DeliveryMethod? DeliveryMethod { get; set; }
    }
}
