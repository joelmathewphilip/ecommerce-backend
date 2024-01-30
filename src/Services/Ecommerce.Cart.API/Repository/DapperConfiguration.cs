using System.Data;
using System.Data.SqlClient;

namespace Ecommerce.Cart.API.Repository
{
    public class DapperConfiguration : IDapper
    {
        public IDbConnection CreateConnection()
        {
            string connString = "";
            return new SqlConnection(connString);
        }
    }
}
