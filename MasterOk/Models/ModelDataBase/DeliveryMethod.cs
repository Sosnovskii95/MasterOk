using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class DeliveryMethod
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Способ доставки")]
        public string TitleDeliveryMethod { get; set; }
    }
}
