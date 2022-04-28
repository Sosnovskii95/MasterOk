using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class StateOrder
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Статус заказа")]
        public string TitleState { get; set; }
    }
}
