namespace Ecommerce.Catalog.API.Models
{
    public class Coupon
    {
        public string couponName { get; set; }
        public Guid catalogId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string couponDescription { get; set; }
        public int discountPercent { get; set; }  
    }
}
