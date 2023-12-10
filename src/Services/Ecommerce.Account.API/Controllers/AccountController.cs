using Ecommerce.Account.API.Interfaces;
using Ecommerce.Account.API.Model;
using Ecommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Ecommerce.Account.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        // GET: api/<CatalogItemsController>
        private readonly IUserRepository _repository;
        private readonly ILogger<AccountController> _logger;
        public ControllerError controllerError;
        public AccountController(IUserRepository repository, ILogger<AccountController> logger)
        {
            _repository = repository;
            _logger = logger;
            controllerError = new ControllerError();
        }
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>?>> GetUsersAsync()
        {
            _logger.LogInformation($"Started executing {nameof(GetUsersAsync)}");
            controllerError = new ControllerError();
            try
            {
                _logger.LogInformation($"Finished executing {nameof(GetUsersAsync)}");
                var result = await _repository.GetUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(GetUsersAsync)));
                controllerError.statusCode = 500;
                controllerError.message = "An error occured while processing";
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);

            }
        }

        // GET api/<AccountController>/5
        [HttpGet("users/{id}")]
        public async Task<ActionResult<User?>> GetUserAsync(Guid id)
        {
            controllerError = new ControllerError();
            try
            {
                _logger.LogInformation($"Started executing {nameof(GetUserAsync)}");
                var result = await _repository.GetUserAsync(id);
                if (result == null)
                {
                    _logger.LogInformation($"Finished executing {nameof(GetUserAsync)}");
                    //return StatusCodeResult(StatusCodes.Status404NotFound, null);
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Finished executing {nameof(GetUserAsync)}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(GetUserAsync)));
                controllerError.statusCode = 500;
                controllerError.message = "An internal error occured while processing";
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }


        // POST api/<AccountController>
        [HttpPost("users")]
        public async Task<ActionResult> AddUserAsync(UserInsertDto userInsertDto)
        {
            _logger.LogInformation($"Started executing {nameof(AddUserAsync)}");
            controllerError = new ControllerError();
            try
            {
                var userItem = userInsertDto.AsUser();
                await _repository.AddUserAsync(userItem);
                _logger.LogInformation($"Finished executing {nameof(AddUserAsync)}");
                return CreatedAtAction(nameof(AddUserAsync), userItem.AsUserInsertDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(AddUserAsync)));
                controllerError.message = "An internal error occured while processing";
                controllerError.statusCode = 500;
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }

        // PUT api/<AccountController>/5
        [HttpPut("users/{id}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
        {
            _logger.LogInformation($"Started executing {nameof(UpdateUserAsync)}");
            controllerError = new ControllerError();
            try
            {
                var oldItem = await _repository.GetUserAsync(id);
                if (oldItem == null)
                {
                    controllerError.statusCode = 404;
                    controllerError.message = "Catalog Item does not exist";
                    _logger.LogInformation($"Finished executing {nameof(UpdateUserAsync)}");
                    return NotFound(controllerError);
                }
                else
                {
                    User NewItem = userUpdateDto.AsUser(oldItem);
                    await _repository.UpdateUserAsync(NewItem);
                    var updatedData = await _repository.GetUserAsync(NewItem.Id);
                    _logger.LogInformation($"Finished executing {nameof(UpdateUserAsync)}");
                    return Ok(updatedData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(UpdateUserAsync)));
                controllerError.message = "An internal error occured while processing";
                controllerError.statusCode = 500;
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            _logger.LogInformation($"Started executing {nameof(DeleteUserAsync)}");
            controllerError = new ControllerError();
            try
            {
                var item = await _repository.GetUserAsync(id);
                if (item == null)
                {
                    controllerError.statusCode = 404;
                    controllerError.message = "Catalog Item does not exist";
                    _logger.LogInformation($"Finished executing {nameof(DeleteUserAsync)}");
                    return NotFound(controllerError);
                }
                else
                {
                    //TO DO
                    //First Delete the orders and cart of a deleted user
                    await _repository.DeleteUserAsync(id);
                    
                    
                    _logger.LogInformation($"Finished executing {nameof(DeleteUserAsync)}");
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Failed to execute  {0}", nameof(DeleteUserAsync)));
                controllerError.message = "An internal error occured while processing";
                controllerError.statusCode = 500;
                return StatusCode(StatusCodes.Status500InternalServerError, controllerError);
            }
        }


    }
}
