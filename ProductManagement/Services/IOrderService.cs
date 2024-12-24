using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using ProductManagement.DTOs;
using ProductManagement.Repositories;

namespace ProductManagement.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<OrderDto> CreateOrderAsync(OrderDto orderDto);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OrderDto> _validator;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IValidator<OrderDto> validator)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);

        }
        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var validateAddressResult = _validator.Validate(orderDto);
            if (!validateAddressResult.IsValid)
            {
                throw new ValidationException(validateAddressResult.Errors);
            }
            var order = _mapper.Map<Order>(orderDto);
            await _orderRepository.CreateOrderAsync(order);
            await _orderRepository.SaveChangesAsync();
            return _mapper.Map<OrderDto>(order);

        }

    }
}
