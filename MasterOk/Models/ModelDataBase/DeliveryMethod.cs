using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class DeliveryMethod
    {
        [Key]
        public int Id { get; set; }

        public string TitleDeliveryMethod { get; set; }
    }
}
