using Catalog.API;
using Ecommerce.Catalog.API.Interfaces;
using Ecommerce.Catalog.API.Models;
using Ecommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Ecommerce.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogItemsController : ControllerBase
    {
        // GET: api/<CatalogItemsController>
        private readonly ICatalogItemRepository _repository;
        private readonly ILogger _logger;
        public ControllerError controllerError;
        public CatalogItemsController(ICatalogItemRepository repository, ILogger<CatalogItemsController> logger)
        {
            _repository = repository;
            _logger = logger;
            controllerError = new ControllerError();
        }
        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<CatalogItem>?>> GetCatalogItemsAsync()
        {
            _logger.LogInformation($"Started executing {nameof(GetCatalogItemsAsync)}");
            controllerError = new ControllerError();
            try
            {    
                var result = await _repository.GetCatalogItemsAsync();
                List<CatalogItemResponseDto> finalList = new List<CatalogItemResponseDto>();
                //finalList = result.ToList();
                if (result is not null)
                {
                    foreach (var item in result)
                    {
                        var discountedPrice = await FetchDiscountedPrice(item);
                        item.DiscountedPrice = (discountedPrice == null) ? item.CatalogMrp : discountedPrice;
                        finalList.Add(item.toDto());
                    }
                }
                _logger.LogInformation($"Finished executing {nameof(GetCatalogItemsAsync)}");
                return Ok(finalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(GetCatalogItemsAsync)));
                controllerError.statusCode = 500;
                controllerError.message = "An error occured while processing";
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);

            }
        }

        // GET api/<CatalogItemsController>/5
        [HttpGet("items/{id}")]
        public async Task<ActionResult<CatalogItem?>> GetCatalogItemAsync(Guid id)
        {
            controllerError = new ControllerError();
            try
            {
                _logger.LogInformation($"Started executing {nameof(GetCatalogItemAsync)}");
                var result = await _repository.GetCatalogItemAsync(id);
                
                if (result == null)
                {
                    _logger.LogInformation($"Finished executing {nameof(GetCatalogItemAsync)}");
                    //return StatusCodeResult(StatusCodes.Status404NotFound, null);
                    return NotFound();
                }
                else
                {
                    var discountedPrice = await FetchDiscountedPrice(result);
                    result.DiscountedPrice = (discountedPrice == null) ? result.CatalogMrp : discountedPrice;
                    _logger.LogInformation($"Finished executing {nameof(GetCatalogItemAsync)}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(GetCatalogItemAsync)));
                controllerError.statusCode = 500;
                controllerError.message = "An internal error occured while processing";
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }


        // POST api/<CatalogItemsController>
        [HttpPost("items")]
        public async Task<ActionResult> AddCatalogItemAsync(CatalogItemInsertRequestDto catalogItemDto)
        {
            _logger.LogInformation($"Started executing {nameof(AddCatalogItemAsync)}");
            controllerError = new ControllerError();
            try
            {
                var catalogItem = catalogItemDto.toItem();
                //var discountedPrice = await FetchDiscountedPrice(catalogItem);
                //catalogItem.DiscountedPrice = (discountedPrice == null) ? catalogItem.CatalogMrp : double.Parse(discountedPrice);
                catalogItem.DiscountedPrice = catalogItem.CatalogMrp;
                await _repository.AddCatalogItemAsync(catalogItem);
                _logger.LogInformation($"Finished executing {nameof(AddCatalogItemAsync)}");
                return CreatedAtAction(nameof(AddCatalogItemAsync), catalogItem.CatalogId, catalogItem.toDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(GetCatalogItemAsync)));
                controllerError.message = "An internal error occured while processing";
                controllerError.statusCode = 500;
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }

        // PUT api/<CatalogItemsController>/5
        [HttpPut("items/{id}")]
        public async Task<ActionResult> UpdateCatalogItemAsync(Guid id, CatalogItemUpdateRequestDto catalogItemDto)
        {
            _logger.LogInformation($"Started executing {nameof(UpdateCatalogItemAsync)}");
            controllerError = new ControllerError();
            try
            {
                var oldItem = await _repository.GetCatalogItemAsync(id);
                if (oldItem == null)
                {
                    controllerError.statusCode = 404;
                    controllerError.message = "Catalog Item does not exist";
                    _logger.LogInformation($"Finished executing {nameof(UpdateCatalogItemAsync)}");
                    return NotFound(controllerError);
                }
                else
                {
                    CatalogItem item = catalogItemDto.toItem(oldItem);
                    item.CatalogId = oldItem.CatalogId;
                    var updatedData = await _repository.UpdateCatalogItemAsync(item);
                    _logger.LogInformation($"Finished executing {nameof(UpdateCatalogItemAsync)}");
                    return Ok(updatedData.toDto());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(UpdateCatalogItemAsync)));
                controllerError.message = "An internal error occured while processing";
                controllerError.statusCode = 500;
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }

        // DELETE api/<CatalogItemsController>/5
        [HttpDelete("items/{id}")]
        public async Task<ActionResult> DeleteCatalogItemAsync(Guid id)
        {
            _logger.LogInformation($"Started executing {nameof(DeleteCatalogItemAsync)}");
            controllerError = new ControllerError();
            try
            {
                var item = await _repository.GetCatalogItemAsync(id);
                if (item == null)
                {
                    controllerError.statusCode = 404;
                    controllerError.message = "Catalog Item does not exist";
                    _logger.LogInformation($"Finished executing {nameof(DeleteCatalogItemAsync)}");
                    return NotFound(controllerError);
                }
                else
                {
                    await _repository.DeleteCatalogItemAsync(id);
                    _logger.LogInformation($"Finished executing {nameof(DeleteCatalogItemAsync)}");
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(DeleteCatalogItemAsync)));
                controllerError.message = "An internal error occured while processing";
                controllerError.statusCode = 500;
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }

        private async Task<dynamic> FetchDiscountedPrice(CatalogItem catalogItem)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"http://localhost:8081/api/discounts?catalogId={catalogItem.CatalogId}");
                //Ensure the operation returned 200 status
                response.EnsureSuccessStatusCode();
                
                //Bind the json response to the a list of Coupon Objects
                List<Coupon> couponObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Coupon>>( await response.Content.ReadAsStringAsync());
                
                int maxDiscount = 0;
                //if there are multiple coupons, loop through and get the maximum coupon.
                foreach(var coupon in couponObjects)
                {
                    maxDiscount = Math.Max(maxDiscount, coupon.discountPercent);
                }
                double discountPercent = maxDiscount;
                //return the final discount amount.
                return  (catalogItem.CatalogMrp - ((discountPercent / 100) * catalogItem.CatalogMrp));
            }
            catch(Exception)
            {
                _logger.LogError($"{nameof(FetchDiscountedPrice)} failed to execute");
                return null;
                
            }
        }
    }
}
