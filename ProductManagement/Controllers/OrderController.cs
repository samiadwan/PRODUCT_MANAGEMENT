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
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<OrderDto> _validator;
        private readonly IMapper _mapper;

        public OrderController(ApplicationDbContext context, IMapper mapper, IValidator<OrderDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }        

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.Include(o => o.User).Include(o => o.OrderItems).ToListAsync();
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return Ok(orderDtos);
        }
 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            var orderDto = _mapper.Map<List<OrderDto>>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            var validationResult = _validator.Validate(orderDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == orderDto.UserId);
            if (!userExists)
            {
                return BadRequest(new
                {
                    Errors = new
                    {
                        UserId = new[] { "The specified UserId does not exist in the database." }
                    }
                });
            }
            var order = _mapper.Map<Order>(orderDto);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var createdOrderDto = _mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, createdOrderDto);
        }
    }
}
