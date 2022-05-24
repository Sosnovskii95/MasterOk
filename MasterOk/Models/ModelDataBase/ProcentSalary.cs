using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class ProcentSalary
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Процент скидки")]
        public int TitleProcentSalary { get; set; }

        [Display(Name = "Порог скидки")]
        public int ValueProcentSalary { get; set; }
    }
}
