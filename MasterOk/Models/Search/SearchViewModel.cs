using MasterOk.Models.ModelDataBase;

namespace MasterOk.Models.Search
{
    public class SearchViewModel
    {
        public ICollection<Product>? Products { get; set; }

        public ICollection<SubCategory>? SubCategories { get; set; }

        public ICollection<Category>? Categories { get; set; }
    }
}
