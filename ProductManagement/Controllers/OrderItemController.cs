using Microsoft.AspNetCore.Mvc;
using ProductManagement.DTOs;
using ProductManagement.Services;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _orderService;
        public OrderItemController(IOrderItemService orderItemService, IOrderService orderService)
        {
            _orderItemService = orderItemService;
            _orderService = orderService;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderItems()
        {
            var orderItems = await _orderItemService.GetOrderItemAsync();     
            return Ok(orderItems);
        }

        [HttpGet("{orderId}/{productId}")]
        public async Task<IActionResult> GetOrderItemById(int orderId, int productId)
        {
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(orderId, productId);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemDto orderItemDto)
        {

            var existingItem = await _orderService.GetOrderByIdAsync((int)orderItemDto.OrderId);

            if (existingItem != null)
            {
                return BadRequest("OrderItem with the same OrderId and ProductId already exists.");
            }
           var createOrderItem = await _orderItemService.CreateOrderItemAsync(orderItemDto);
           return CreatedAtAction(nameof(GetOrderItemById), new { orderId = createOrderItem.OrderId, productId = createOrderItem.ProductId }, createOrderItem);
        }
    }
}
