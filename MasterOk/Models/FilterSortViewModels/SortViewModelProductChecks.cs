using MasterOk.Models.ModelDataBase;
using MasterOk.Models.FilterSortViewModels;
namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelProductChecks
    {
        public ICollection<ProductCheck> ProductChecks { get; set; }

        public SortModelProductChecks SortModelProductChecks { get; set; }
    }
}
