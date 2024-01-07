using Ecommerce.Orders.Application.Features.Orders.Commands.CheckoutOrder;
using Ecommerce.Orders.Application.Features.Orders.Commands.DeleteOrdder;
using Ecommerce.Orders.Application.Features.Orders.Commands.UpdateOrder;
using Ecommerce.Orders.Application.Features.Orders.Queries.GetOrdersList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [HttpGet("{username}", Name = "Get Order")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string username)
        {
            var query = new GetOrdersListQuery(username);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost(Name = "Checkout Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand checkoutOrderCommand)
        {
            //var query = new GetOrdersListQuery(username);
            var response = await _mediator.Send(checkoutOrderCommand);
            return Ok(response);
        }


        [HttpPost(Name = "Update Order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Unit>> UpdateOrder([FromBody] UpdateOrderCommand updateOrderCommand)
        {
            var response = await _mediator.Send(updateOrderCommand);
            return NoContent();
        }

        [HttpPost("{id",Name = "Delete Order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Unit>> DeleteOrder(int Id)
        {
            var deleteOrderCommand = new DeleteOrderCommand() { Id = Id };
            var response = await _mediator.Send(deleteOrderCommand);
            return NoContent();
        }
    }
}
