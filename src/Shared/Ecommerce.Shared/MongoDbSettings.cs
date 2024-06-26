﻿namespace Ecommerce.Shared
{
    public class MongoDbSettings
    {
        public string mongoHost { get; set; }
        public int mongoPort { get; set; }   
        public string mongoUsername { get; set; }
        public string mongoPassword { get; set; }
        public string connectionString
        {
            get
            {
                return $"mongodb://{mongoUsername}:{mongoPassword}@{mongoHost}:{mongoPort}";

            }
        }
    }
}
