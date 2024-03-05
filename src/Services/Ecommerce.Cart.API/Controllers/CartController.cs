using Ecommerce.Cart.API.Entity;
using Ecommerce.Cart.API.Repository;
using Ecommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Ecommerce.Cart.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        private readonly ILogger<CartController> _logger;
        private ControllerError _controllerError;
        private readonly IConfiguration _config;
        public CartController(ILogger<CartController> logger, ICartRepository repository, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _controllerError = new ControllerError();
            _config = configuration;
        }


        [HttpGet("{cartId}/quantity", Name = "Get Brief Cart")]
        public async Task<ActionResult<CartCount>> GetCartQuantity(string cartId)
        {
            try
            {

                if (await _repository.CartExists(cartId) < 1)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Cart Does not exist");
                }
                var result = await _repository.GetCartQuantity(cartId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _controllerError.statusCode = 500;
                _controllerError.message = ex.Message.ToString();
                _controllerError.errorObject = null;
                return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
            }
        }

        [HttpGet("{cartId}", Name = "Get Cart")]
        public async Task<ActionResult<CartEntity>> GetCart(string cartId)
        {
            try
            {
                if (await _repository.CartExists(cartId) < 1)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Cart Does not exist");
                }

                var result = await _repository.GetCartAll(cartId);
                if (result != null)
                {
                    foreach (var item in result.cartItems)
                    {
                        var response = await fetchItemCost(item);
                        item.itemcost = response.itemcost;
                        result.totalCost += item.itemquantity * item.itemcost;
                    }
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _controllerError.statusCode = 500;
                _controllerError.message = ex.Message.ToString();
                _controllerError.errorObject = null;
                return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
            }

        }

        [HttpPost("{cartId}", Name = "Add to Cart")]
        public async Task<ActionResult<CartCount>> AddToCart([FromBody] CartItemDto cartItemDto, string cartId)
        {
            try
            {
                var cartItem = CartExtensions.asCartItem(cartItemDto);


                if (await _repository.CartExists(cartId) < 1)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Cart Does not exist");
                }

                var cart = await _repository.AddItem(cartItem, cartId);
                return Ok(await _repository.GetCartQuantity(cartId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _controllerError.statusCode = 500;
                _controllerError.message = ex.Message.ToString();
                _controllerError.errorObject = null;
                return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
            }
        }

        /*[HttpPost("{id}/checkout")]
        public async Task<ActionResult> Checkout()
        {
            try
            {

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }*/


        [HttpPost("{cartId}/removeitem")]
        public async Task<ActionResult> RemoveFromCart([FromBody] string itemId, string cartId)
        {
            try
            {
                if (await _repository.CartExists(cartId) < 1)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Cart Does not exist");
                }

                int resp = await _repository.DeleteItem(cartId, itemId);
                if (resp == 0)
                {
                    _controllerError.statusCode = 500;
                    _controllerError.message = "No such items to remove";
                    _controllerError.errorObject = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _controllerError.statusCode = 500;
                _controllerError.message = ex.Message.ToString();
                _controllerError.errorObject = ex;
                return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
            }
        }

        private async Task<CartItem> fetchItemCost(CartItem cartItem)
        {
            try
            {
                string catalogUrl = _config["CatalogUrl"] + "/" + cartItem.itemid;
                HttpClient httpClient = new HttpClient();
                var token = await fetchToken();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await httpClient.GetAsync(catalogUrl);
                response.EnsureSuccessStatusCode();
                var output = JObject.Parse(await response.Content.ReadAsStringAsync());
                cartItem.itemcost = double.Parse(output.Value<string>("discountedPrice"));
                return cartItem;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while fetching catalog Costs: " + ex.Message);
                throw new Exception("Failed to fetch CatalogCost");
            }

        }
        private async Task<string> fetchToken()
        {
            string identityUrl = _config["IdentityUrl"];
            HttpClient client = new HttpClient();
            var body = new { username = "string", password = "string" };
            var response = await client.PostAsync(identityUrl, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var output = JObject.Parse(await response.Content.ReadAsStringAsync());
            return output.Value<string>("jwtToken");
        }
    }
}
