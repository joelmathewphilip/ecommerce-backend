using Ecommerce.Orders.Application.Features.Orders.Commands.CheckoutOrder;
using Ecommerce.Orders.Application.Features.Orders.Commands.DeleteOrder;
using Ecommerce.Orders.Application.Features.Orders.Commands.UpdateOrder;
using Ecommerce.Orders.Application.Features.Orders.Queries.GetAllOrdersList;
using Ecommerce.Orders.Application.Features.Orders.Queries.GetOrdersList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Orders.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("{username}", Name = "Get Order")]
        [ProducesResponseType(typeof(IEnumerable<Application.Features.Orders.Queries.GetOrdersList.OrdersVm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Application.Features.Orders.Queries.GetOrdersList.OrdersVm>>> GetOrdersByUserName(string username)
        {
            var query = new GetOrdersListQuery(username);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles ="admin")]
        [HttpGet(Name ="Get All Orders")]
        [ProducesResponseType(typeof(IEnumerable<Application.Features.Orders.Queries.GetAllOrdersList.OrdersVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Application.Features.Orders.Queries.GetAllOrdersList.OrdersVm>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Application.Features.Orders.Queries.GetAllOrdersList.OrdersVm>>> GetAllOrders()
        {
            var query = new GetAllOrdersListQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [Authorize]
        [HttpPost(Name = "Checkout Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand checkoutOrderCommand)
        {
            //var query = new GetOrdersListQuery(username);
            var response = await _mediator.Send(checkoutOrderCommand);
            return Ok(checkoutOrderCommand);
        }

        [HttpPut(Name = "Update Order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Unit>> UpdateOrder([FromBody] UpdateOrderCommand updateOrderCommand)
        {
            var response = await _mediator.Send(updateOrderCommand);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}",Name = "Delete Order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Unit>> DeleteOrder(int id)
        {
            var deleteOrderCommand = new DeleteOrderCommand() { Id = id };
            var response = await _mediator.Send(deleteOrderCommand);
            return NoContent();
        }
    }
}
