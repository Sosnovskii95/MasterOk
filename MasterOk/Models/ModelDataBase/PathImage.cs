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

    public static class PathImageExtensions
    {
        private static string DirectoryFile = "/Content/";
        private static string PathNameImage = "imagenot.jpg";
        private static string TypeImage = "image/jpg";

        public static string GetPathNameImage()
        {
            return PathNameImage;
        }

        public static string GetTypeImage()
        {
            return TypeImage;
        }

        public static string GetDirectoryFile()
        {
            return DirectoryFile;
        }

        public static PathImage GetDefaultPathNameFile(Category category)
        {
            return new PathImage { Category = category, PathNameImage = PathNameImage, TypeImage = TypeImage };
        }

        public static PathImage GetDefaultPathNameFile(SubCategory subCategory)
        {
            return new PathImage { SubCategory = subCategory, PathNameImage = PathNameImage, TypeImage = TypeImage };
        }

        public static PathImage GetDefaultPathNameFile(Product product)
        {
            return new PathImage { Product = product, PathNameImage = PathNameImage, TypeImage = TypeImage };
        }

        public static string GetDirectorySaveFile(Category category)
        {
            return "/Content/Category/";
        }
        
        public static string GetDirectorySaveFile(SubCategory subCategory)
        {
            return "/Content/SubCategory/";
        }
        
        public static string GetDirectorySaveFile(Product product)
        {
            return "/Content/Product/";
        }
    }
}
