using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared
{
    public class PostgresDbSettings
    {
        public PostgresDbSettings()
        {
        }
        public string postgresPort { get; set; }
        public string postgresDb { get; set; }
        public string postgresUser { get; set; }
        public string postgresPassword { get; set; }
        public string postgresHost { get; set; }

        public string connString
        {
            get
            {
                return $"Server = {postgresHost}; Port = {postgresPort}; Database = {postgresDb}; User Id = {postgresUser}; Password = {postgresPassword};";

            }
        }
    }
}
