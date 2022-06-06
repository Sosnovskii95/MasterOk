using MasterOk.Models.ModelDataBase;
namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelClient
    {
        public ICollection<Client> Clients { get; set; }

        public SortModelClient SortModelClient { get; set; }
    }
}
