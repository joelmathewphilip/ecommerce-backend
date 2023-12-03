using Ecommerce.Catalog.API.Interfaces;
using Ecommerce.Catalog.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Catalog.API.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DiscountService> _logger;
        public DiscountService(IConfiguration configuration, ILogger<DiscountService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<dynamic> FetchDiscountedPrice(CatalogItem catalogItem)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var requestUrl = _configuration["CouponService:Url"] + $"?catalogId={catalogItem.CatalogId}";
                var response = await httpClient.GetAsync(requestUrl);
                //Ensure the operation returned 200 status
                //response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    //Bind the json response to the a list of Coupon Objects
                    List<Coupon> couponObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Coupon>>(await response.Content.ReadAsStringAsync());

                    int maxDiscount = 0;
                    //if there are multiple coupons, loop through and get the maximum coupon.
                    foreach (var coupon in couponObjects)
                    {
                        maxDiscount = Math.Max(maxDiscount, coupon.discountPercent);
                    }
                    double discountPercent = maxDiscount;

                    //return the final discount amount.
                    return (catalogItem.CatalogMrp - ((discountPercent / 100) * catalogItem.CatalogMrp));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                _logger.LogError($"{nameof(FetchDiscountedPrice)} failed to execute");
                throw;

            }
        }
    }
}
