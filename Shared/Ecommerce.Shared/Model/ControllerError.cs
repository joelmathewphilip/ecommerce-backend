namespace Ecommerce.Shared.Models
{
    public class ControllerError
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public dynamic? errorObject { get; set; }
    }
}
