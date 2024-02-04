using Ecommerce.Cart.API.Entity;
using Ecommerce.Cart.API.Repository;
using Ecommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Cart.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        private readonly ILogger<CartController> _logger;
        private ControllerError _controllerError;
        public CartController(ILogger<CartController> logger, ICartRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _controllerError = new ControllerError();
        }

        /*[Authorize]

        [HttpGet(Name ="Get All Cart Details")]
        public async Task<ActionResult<IEnumerable<CartEntity>>> GetAllCart()
        {

        }*/

        [HttpGet("{id}", Name = "Get Cart")]
        public async Task<ActionResult<CartEntity>> GetCart(string cartId)
        {
            try
            {
                if (await _repository.CartExists(cartId) < 1)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Cart Does not exist");
                }

                var result = await _repository.GetCart(cartId);
                if (result == null)
                {
                    _controllerError.statusCode = 500;
                    _controllerError.message = "Cart does not exist";
                    _controllerError.errorObject = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, _controllerError);
                }
                else
                {
                    return Ok(result);
                }


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

        [HttpPost("{id}", Name = "Add to Cart")]
        public async Task<ActionResult> AddToCart([FromBody] CartItem cartItem, string cartId)
        {
            try
            {
                if (await _repository.CartExists(cartId) < 1)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Cart Does not exist");
                }

                var cart = await _repository.AddItem(cartItem, cartId);
                return Ok(cart);
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


        [HttpPost("{cartid}/removeitem")]
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
    }
}
