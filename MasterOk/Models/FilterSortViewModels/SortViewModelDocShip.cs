using MasterOk.Models.ModelDataBase;
using MasterOk.Models.FilterSortViewModels;
namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelDocShip
    {
        public ICollection<DocShipToStore> DocShipToStores { get; set; }

        public SortModelDocShip SortModelDocShip { get; set; }
    }
}
