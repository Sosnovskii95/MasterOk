using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class CartClient
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Количество")]
        public int CountCartProduct { get; set; }

        [Display(Name = "Товар")]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        public Client? Client { get; set; }
    }
}
