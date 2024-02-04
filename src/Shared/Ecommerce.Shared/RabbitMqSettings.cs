namespace Ecommerce.Shared
{
    public class RabbitMqSettings
    {
        public string username { get; set; }
        public string password { get; set; }

        public string hostname { get; set; }

        public string port { get; set; }

        public string connString
        {
            get
            {
                return $"rabbitmq://{username}:{password}@{hostname}:{port}";
            }
        }


       
    }
}
