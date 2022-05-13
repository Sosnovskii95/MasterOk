namespace MasterOk.Models.FilterSortViewModels
{
    public class SortModelProductChecks
    {
        public ESortModelProductCheck IdSort { get; private set; }

        public ESortModelProductCheck DateTimeSaleSort { get; private set; }

        public ESortModelProductCheck StateOrderSort { get; private set; }

        public ESortModelProductCheck ClientSort { get; private set; }

        public ESortModelProductCheck UserSort { get; private set; }

        public ESortModelProductCheck PayMethodSort { get; private set; }

        public ESortModelProductCheck DeliveryMethodSort { get; private set; }

        public ESortModelProductCheck Current { get; private set; }

        public SortModelProductChecks(ESortModelProductCheck sort)
        {
            IdSort = sort == ESortModelProductCheck.IdAsc ? ESortModelProductCheck.IdDesc : ESortModelProductCheck.IdAsc;
            DateTimeSaleSort = sort == ESortModelProductCheck.DateTimeSaleAsc ? ESortModelProductCheck.DateTimeSaleDesc : ESortModelProductCheck.DateTimeSaleAsc;
            StateOrderSort = sort == ESortModelProductCheck.StateOrderIdAsc ? ESortModelProductCheck.StateOrderIdDesc : ESortModelProductCheck.StateOrderIdAsc;
            ClientSort = sort == ESortModelProductCheck.ClientIdAsc ? ESortModelProductCheck.ClientIdDesc : ESortModelProductCheck.ClientIdAsc;
            UserSort = sort == ESortModelProductCheck.UserIdAsc ? ESortModelProductCheck.UserIdDesc : ESortModelProductCheck.UserIdAsc;
            PayMethodSort = sort == ESortModelProductCheck.PayMethodIdAsc ? ESortModelProductCheck.PayMethodIdDesc : ESortModelProductCheck.PayMethodIdAsc;
            DeliveryMethodSort = sort == ESortModelProductCheck.DeliveryMethodIdAsc ? ESortModelProductCheck.DeliveryMethodIdDesc : ESortModelProductCheck.DeliveryMethodIdAsc;
            Current = sort;
        }
    }

    public enum ESortModelProductCheck
    {
        IdAsc,
        IdDesc,
        DateTimeSaleAsc,
        DateTimeSaleDesc,
        StateOrderIdAsc,
        StateOrderIdDesc,
        ClientIdAsc,
        ClientIdDesc,
        UserIdAsc,
        UserIdDesc,
        PayMethodIdAsc,
        PayMethodIdDesc,
        DeliveryMethodIdAsc,
        DeliveryMethodIdDesc
    }
}
