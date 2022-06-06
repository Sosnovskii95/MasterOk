using MasterOk.Models.ModelDataBase;
namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelProduct
    {
        public ICollection<Product> Products { get; set; }

        public SortModelProduct SortModelProduct { get; set; }
    }
}
