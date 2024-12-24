using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using ProductManagement.DTOs;
using ProductManagement.Repositories;

namespace ProductManagement.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetOrderItemAsync();
        Task<OrderItemDto> GetOrderItemByIdAsync(int orderId, int productId);
        Task<OrderItemDto> CreateOrderItemAsync(OrderItemDto itemDto);
    }

    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OrderItemDto> _validator;
        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper, IValidator<OrderItemDto> validator)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _validator = validator;
            
        }

        public async Task<OrderItemDto> CreateOrderItemAsync(OrderItemDto itemDto)
        {
            var validateAddressResult = _validator.Validate(itemDto);
            if (!validateAddressResult.IsValid)
            {
                throw new ValidationException(validateAddressResult.Errors);
            }
            var item = _mapper.Map<OrderItem>(itemDto);
            await _orderItemRepository.CreateOrderItemAsync(item);
            await _orderItemRepository.SaveChangesAsync();
            return _mapper.Map<OrderItemDto>(item);
        }

        public async Task<IEnumerable<OrderItemDto>> GetOrderItemAsync()
        {
            var orderItem = await _orderItemRepository.GetOrderItemAsync();
            return _mapper.Map<IEnumerable<OrderItemDto>>(orderItem);
        }

        public async Task<OrderItemDto> GetOrderItemByIdAsync(int orderId, int productId)
        {

            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(orderId, productId);
            return _mapper.Map<OrderItemDto>(orderItem);
        }

    }
}
