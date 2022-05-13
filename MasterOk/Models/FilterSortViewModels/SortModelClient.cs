namespace MasterOk.Models.FilterSortViewModels
{
    public class SortModelClient
    {
        public ESortModelClient IdSort { get; private set; }
        
        public ESortModelClient EmailSort { get; private set; }
        
        public ESortModelClient NameSort { get; private set; }
        
        public ESortModelClient NumberPhoneSort { get; private set; }
        
        public ESortModelClient AddressSort { get; private set; }
        
        public ESortModelClient SalarySort { get; private set; }
        
        public ESortModelClient Current { get; private set; }

        public SortModelClient(ESortModelClient sort)
        {
            IdSort = sort == ESortModelClient.IdAsc ? ESortModelClient.IdDesc : ESortModelClient.IdAsc;
            EmailSort = sort == ESortModelClient.EmailAsc ? ESortModelClient.EmailDesc : ESortModelClient.EmailAsc;
            NameSort = sort == ESortModelClient.NameAsc ? ESortModelClient.NameDesc : ESortModelClient.NameAsc;
            NumberPhoneSort = sort == ESortModelClient.NumberPhoneAsc ? ESortModelClient.NumberPhoneDesc : ESortModelClient.NumberPhoneAsc;
            AddressSort = sort == ESortModelClient.AddressAsc ? ESortModelClient.AddressDesc : ESortModelClient.AddressAsc;
            SalarySort = sort == ESortModelClient.SalaryAsc ? ESortModelClient.SalaryDesc : ESortModelClient.AddressAsc;
            Current = sort;
        }
    }

    public enum ESortModelClient
    {
        IdAsc,
        IdDesc,
        EmailAsc,
        EmailDesc,
        NameAsc,
        NameDesc,
        NumberPhoneAsc,
        NumberPhoneDesc,
        AddressAsc,
        AddressDesc,
        SalaryAsc,
        SalaryDesc
    }
}
