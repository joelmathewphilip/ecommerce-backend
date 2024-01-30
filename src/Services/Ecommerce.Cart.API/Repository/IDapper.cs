using System.Data;
using System.Data.SqlClient;

namespace Ecommerce.Cart.API.Repository
{
    public interface IDapper
    {
        public IDbConnection CreateConnection();
    }
}
