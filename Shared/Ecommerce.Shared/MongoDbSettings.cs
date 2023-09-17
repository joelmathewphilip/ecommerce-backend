namespace Ecommerce.Shared
{
    public class MongoDbSettings
    {
        public string mongoHost { get; set; }
        public int mongoPort { get; set; }   
        public string mongoUsername { get; set; }
        public string mongoPassword { get; set; }
        public string mongoDbName { get; set; }
        public string connectionString
        {
            get
            {
                //return $"mongodb://{username}:{password}@{host}:{port}";
                //return $"mongodb+srv://{mongoUsername}:{mongoPassword}@{mongoHost}";
                return $"mongodb://{mongoUsername}:{mongoPassword}@{mongoHost}:{mongoPort}/{mongoDbName}?authSource=admin";

            }
        }
    }
}
