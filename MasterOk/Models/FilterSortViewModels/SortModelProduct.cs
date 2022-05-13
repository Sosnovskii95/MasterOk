namespace MasterOk.Models.FilterSortViewModels
{
    public class SortModelProduct
    {
        public ESortModelProduct IdSort { get; private set; }

        public ESortModelProduct TitleSort { get; private set; }

        public ESortModelProduct DescriptionSort { get; private set; }

        public ESortModelProduct WarrantySort { get; private set; }

        public ESortModelProduct PriceSort { get; private set; }

        public ESortModelProduct SubCategorySort { get; private set; }

        public ESortModelProduct CountSort { get; private set; }

        public ESortModelProduct Current { get; private set; }

        public SortModelProduct(ESortModelProduct sort)
        {
            IdSort = sort == ESortModelProduct.IdAsc ? ESortModelProduct.IdDesc : ESortModelProduct.IdAsc;
            TitleSort = sort == ESortModelProduct.TitleAsc ? ESortModelProduct.TitleDesc : ESortModelProduct.TitleAsc;
            DescriptionSort = sort == ESortModelProduct.DescriptionAsc ? ESortModelProduct.DescriptionDesc : ESortModelProduct.DescriptionAsc;
            WarrantySort = sort == ESortModelProduct.WarrantyAsc ? ESortModelProduct.WarrantyDesc : ESortModelProduct.WarrantyAsc;
            PriceSort = sort == ESortModelProduct.PriceAsc ? ESortModelProduct.PriceDesc : ESortModelProduct.PriceAsc;
            SubCategorySort = sort == ESortModelProduct.SubCategoryAsc ? ESortModelProduct.SubCategoryDesc : ESortModelProduct.SubCategoryAsc;
            CountSort = sort == ESortModelProduct.CountAsc ? ESortModelProduct.CountDesc : ESortModelProduct.CountAsc;
            Current = sort;
        }
    }

    public enum ESortModelProduct
    {
        IdAsc,
        IdDesc,
        TitleAsc,
        TitleDesc,
        DescriptionAsc,
        DescriptionDesc,
        WarrantyAsc,
        WarrantyDesc,
        PriceAsc,
        PriceDesc,
        SubCategoryAsc,
        SubCategoryDesc,
        CountAsc,
        CountDesc
    }
}
