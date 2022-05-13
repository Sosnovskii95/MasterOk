using MasterOk.Models.ModelDataBase;

namespace MasterOk.Models.FilterSortViewModels
{
    public class SortViewModelUsers
    {
        public ICollection<User> Users { get; set; }

        public SortModelUsers SortModelUsers { get; set; }
    }
}
