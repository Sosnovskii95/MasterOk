using MasterOk.Models.ModelDataBase;
using MasterOk.Models.FilterSortViewModels;
namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelProduct
    {
        public ICollection<Product> Products { get; set; }

        public SortModelProduct SortModelProduct { get; set; }
    }
}
