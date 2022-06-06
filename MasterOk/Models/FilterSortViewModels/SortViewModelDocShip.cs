using MasterOk.Models.ModelDataBase;
namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelDocShip
    {
        public ICollection<DocShipToStore> DocShipToStores { get; set; }

        public SortModelDocShip SortModelDocShip { get; set; }
    }
}
