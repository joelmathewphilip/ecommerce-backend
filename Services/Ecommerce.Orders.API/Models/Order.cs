
using Ecommerce.Catalog.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Orders.API
{
    public class Order
    {
        public Guid OrderId {  get; set; }
        public int OrderQuantity { get; set; }
        public double OrderCost { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public Guid OrderPaymentId { get; set; }    

        public List<CatalogItem> CatalogItem { get; set; }   
    }
}
