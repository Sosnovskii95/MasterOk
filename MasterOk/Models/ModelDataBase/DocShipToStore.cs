using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class DocShipToStore
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Дата отгрузки")]
        public DateTime DateShip { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        public ICollection<ShipToStore>? ShipToStores { get; set; }
    }
}
