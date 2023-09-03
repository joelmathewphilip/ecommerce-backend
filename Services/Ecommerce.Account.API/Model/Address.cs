namespace Ecommerce.Account.API.Model
{
    public class Address
    {
        public string StreetName { get; set; }   
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public long ZipCode { get; set; }   
        public string State { get; set; }   
        public string Country { get; set; }
    }
}
