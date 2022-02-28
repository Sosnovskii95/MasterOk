using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Position
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name ="Название должности")]
        public string TitlePosition { get; set; }
    }
}
