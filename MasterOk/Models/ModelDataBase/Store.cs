using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Товар")]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [Display(Name = "Количество на складе")]
        public int CountStoreProduct { get; set; }
    }
}
