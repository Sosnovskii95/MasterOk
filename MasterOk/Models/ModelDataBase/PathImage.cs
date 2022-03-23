using System.ComponentModel.DataAnnotations;

namespace MasterOk.Models.ModelDataBase
{
    public class PathImage
    {
        [Key]
        public int Id { get; set; }
                
        public int? ProductId { get; set; }

        public int? SubCategoryId { get; set; }

        public int? CategoryId { get; set; }

        public string PathNameImage { get; set; }

        public string TypeImage { get; set; }

        public Product Product { get; set; }

        public SubCategory SubCategory { get; set; }

        public Category Category { get; set; }
    }
}
