using MasterOk.Models.ModelDataBase;

namespace MasterOk.Models.ModelAuthorization
{
    public class CartRegisterModel
    {
        public IEnumerable<CartClient> CartClients { get; set; }

        public RegisterModel? RegisterModel { get; set; }
    }
}
