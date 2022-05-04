using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class DocShipToStore
    {
        [Key]
        [Display(Name = "Номер отгрузки")]
        public int Id { get; set; }

        [Display(Name = "Дата отгрузки")]
        public DateTime DateShip { get; set; }

        [Display(Name = "Менеджер")]
        public int? UserId { get; set; }

        [Display(Name = "Менеджер")]
        public User? User { get; set; }

        public ICollection<ShipToStore>? ShipToStores { get; set; }
    }
}
