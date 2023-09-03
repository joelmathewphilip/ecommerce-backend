using Catalog.API;
using Ecommerce.Catalog.API.Interfaces;
using Ecommerce.Catalog.API.Models;
using Ecommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;

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
                _logger.LogInformation($"Finished executing {nameof(GetCatalogItemsAsync)}");
                var result = await _repository.GetCatalogItemsAsync();
                return Ok(result);
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
        public async Task<ActionResult> AddCatalogItemAsync(CatalogItemInsertDto catalogItemDto)
        {
            _logger.LogInformation($"Started executing {nameof(AddCatalogItemAsync)}");
            controllerError = new ControllerError();
            try
            {
                var catalogItem = catalogItemDto.asItem();
                await _repository.AddCatalogItemAsync(catalogItem);
                _logger.LogInformation($"Finished executing {nameof(AddCatalogItemAsync)}");
                return CreatedAtAction(nameof(AddCatalogItemAsync), catalogItem.CatalogId, catalogItem.asDto());
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
        public async Task<ActionResult> UpdateCatalogItemAsync(Guid id, CatalogItemUpdateDto catalogItemDto)
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
                    CatalogItem item = catalogItemDto.asItem(oldItem);
                    item.CatalogId = oldItem.CatalogId;
                    await _repository.UpdateCatalogItemAsync(item);
                    var updatedData = await _repository.GetCatalogItemAsync(id);
                    _logger.LogInformation($"Finished executing {nameof(UpdateCatalogItemAsync)}");
                    return Ok(updatedData);
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
    }
}
