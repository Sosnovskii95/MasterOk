namespace MasterOk.Models.FilterSortViewModels
{
    public class SortModelUsers
    {
        public ESortModelUsers IdSort { get; private set; }

        public ESortModelUsers EmailSort { get; private set; }

        public ESortModelUsers LoginSort { get; private set; }

        public ESortModelUsers NameSort { get; private set; }

        public ESortModelUsers AgeSort { get; private set; }

        public ESortModelUsers NumberPhoneSort { get; private set; }

        public ESortModelUsers ActiveSort { get; private set; }

        public ESortModelUsers Current { get; private set; }

        public SortModelUsers(ESortModelUsers sort)
        {
            IdSort = sort == ESortModelUsers.IdAsc ? ESortModelUsers.IdDesc : ESortModelUsers.IdAsc;
            EmailSort = sort == ESortModelUsers.EmailAsc ? ESortModelUsers.EmailDesc : ESortModelUsers.EmailDesc;
            LoginSort = sort == ESortModelUsers.LoginAsc ? ESortModelUsers.LoginDesc : ESortModelUsers.LoginAsc;
            NameSort = sort == ESortModelUsers.NameAsc ? ESortModelUsers.NameDesc : ESortModelUsers.NameAsc;
            AgeSort = sort == ESortModelUsers.AgeAsc ? ESortModelUsers.AgeDesc : ESortModelUsers.AgeAsc;
            NumberPhoneSort = sort == ESortModelUsers.NumberPhoneAsc ? ESortModelUsers.NumberPhoneDesc : ESortModelUsers.NumberPhoneAsc;
            ActiveSort = sort == ESortModelUsers.ActiveAsc ? ESortModelUsers.ActiveDesc : ESortModelUsers.ActiveAsc;
            Current = sort;
        }
    }

    public enum ESortModelUsers
    {
        IdAsc,
        IdDesc,
        EmailAsc,
        EmailDesc,
        LoginAsc,
        LoginDesc,
        NameAsc,
        NameDesc,
        AgeAsc,
        AgeDesc,
        NumberPhoneAsc,
        NumberPhoneDesc,
        ActiveAsc,
        ActiveDesc
    }
}
