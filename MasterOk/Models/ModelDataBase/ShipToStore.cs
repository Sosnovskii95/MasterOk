using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class ShipToStore
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Количество товара")]
        public int CountShipProduct { get; set; }

        [Display(Name = "Товар")]
        public int ProductId { get; set; }

        [Display(Name = "Товар")]
        public Product Product { get; set; }

        [Display(Name = "Номер отгрузки")]
        public int DocShipToStoreId { get; set; }

        [Display(Name = "Номер отгрузки")]
        public DocShipToStore DocShipToStore { get; set; }
    }
}
