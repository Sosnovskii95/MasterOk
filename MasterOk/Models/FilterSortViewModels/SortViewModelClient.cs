using MasterOk.Models.ModelDataBase;
using MasterOk.Models.FilterSortViewModels;
namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelClient
    {
        public ICollection<Client> Clients { get; set; }

        public SortModelClient SortModelClient { get; set; }
    }
}
