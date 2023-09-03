namespace Ecommerce.Shared
{
    public class MongoDbSettings
    {
        public string mongoHost { get; set; }
        public int port { get; set; }   
        public string mongoUsername { get; set; }
        public string mongoPassword { get; set; }
        public string connectionString
        {
            get
            {
                //return $"mongodb://{username}:{password}@{host}:{port}";
                return $"mongodb+srv://{mongoUsername}:{mongoPassword}@{mongoHost}/?retryWrites=true&w=majority";
            }
        }
    }
}
