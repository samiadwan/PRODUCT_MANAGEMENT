using AutoMapper;
using DataAccessLayer.AccessLayer;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DTOs;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<OrderItemDto> _validator;
        private readonly IMapper _mapper;
        public OrderItemController(ApplicationDbContext context, IMapper mapper, IValidator<OrderItemDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderItems()
        {
            var orderItems = await _context.OrderItem.ToListAsync();
            var orderItemDtos = _mapper.Map<List<OrderItemDto>>(orderItems);
            return Ok(orderItemDtos);
        }

        [HttpGet("{orderId}/{productId}")]
        public async Task<IActionResult> GetOrderItem(int orderId, int productId)
        {
            var orderItem = await _context.OrderItem.FindAsync(orderId, productId);
            if (orderItem == null)
            {
                return NotFound();
            }
            var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemDto orderItemDto)
        {
            var validationResult = _validator.Validate(orderItemDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var existingItem = await _context.OrderItem.FirstOrDefaultAsync(oi => oi.OrderId == orderItemDto.OrderId && oi.ProductId == orderItemDto.ProductId);

            if (existingItem != null)
            {
                return BadRequest("OrderItem with the same OrderId and ProductId already exists.");
            }

            var newOrderItem = _mapper.Map<OrderItem>(orderItemDto);
            _context.OrderItem.Add(newOrderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { orderId = newOrderItem.OrderId, productId = newOrderItem.ProductId }, orderItemDto);
        }
    }
}
