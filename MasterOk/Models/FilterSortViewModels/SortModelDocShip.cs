namespace MasterOk.Models.FilterSortViewModels
{
    public class SortModelDocShip
    {
        public ESortModelDocShip IdSort { get; private set; }

        public ESortModelDocShip DateSort { get; private set; }

        public ESortModelDocShip UserSort { get; private set; }

        public ESortModelDocShip Current { get; private set; }

        public SortModelDocShip(ESortModelDocShip sort)
        {
            IdSort = sort == ESortModelDocShip.IdAsc ? ESortModelDocShip.IdDesc : ESortModelDocShip.IdAsc;
            DateSort = sort == ESortModelDocShip.DateAsc ? ESortModelDocShip.DateDesc : ESortModelDocShip.DateAsc;
            UserSort = sort == ESortModelDocShip.UserAsc ? ESortModelDocShip.UserDesc : ESortModelDocShip.UserAsc;
            Current = sort;
        }
    }

    public enum ESortModelDocShip
    {
        IdAsc,
        IdDesc,
        DateAsc,
        DateDesc,
        UserAsc,
        UserDesc
    }
}
