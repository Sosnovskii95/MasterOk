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
        public int StateOrderId { get; set; }

        [Display(Name = "Статус заказа")]
        public StateOrder? StateOrder { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        [Display(Name = "Клиент")]
        public Client? Client { get; set; }

        [Display(Name = "Менеджер")]
        public int? UserId { get; set; }

        [Display(Name = "Менеджер")]
        public User? User { get; set; }

        public ICollection<ProductSold> ProductSolds { get; set; }

        [Display(Name = "Способ оплаты")]
        public int? PayMethodId { get; set; }

        [Display(Name = "Способ оплаты")]
        public PayMethod? PayMethod { get; set; }

        [Display(Name = "Способ доставки")]
        public int? DeliveryMethodId { get; set; }

        [Display(Name = "Способ доставки")]
        public DeliveryMethod? DeliveryMethod { get; set; }
    }
}
