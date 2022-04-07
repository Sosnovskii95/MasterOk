using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class PayMethod
    {
        [Key]
        public int Id { get; set; }

        public string TitlePayMethod { get; set; }
    }
}
