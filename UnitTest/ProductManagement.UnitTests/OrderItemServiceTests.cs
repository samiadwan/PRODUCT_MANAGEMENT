using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using Moq;
using ProductManagement.DTOs;
using ProductManagement.Repositories;
using ProductManagement.Services;
using ProductManagement.Validators;

namespace ProductManagement.UnitTests
{
    public class OrderItemServiceTests
    {
        private readonly Mock<IOrderItemRepository> _mockOrderItemRepository;
        private readonly OrderItemService _orderItemService;
        private readonly Mock<IMapper> _mapper;
        private readonly OrderItemValidator _validator;
        public OrderItemServiceTests() 
        {
            _mapper = new Mock<IMapper>();
            _validator = new OrderItemValidator();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();
            _orderItemService = new OrderItemService(_mockOrderItemRepository.Object, _mapper.Object, _validator);
        }
        [Fact]
        public async Task GetAllOrderItemsAsync_ReturnsAllOrderItems()
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem { OrderId = 1, ProductId = 1, Quantity= 2},
                new OrderItem { OrderId = 1, ProductId = 1, Quantity= 2},
            };
            var orderItemsDto = new List<OrderItemDto>
            {
                new OrderItemDto { OrderId = 1, ProductId = 1, Quantity= 2},
                new OrderItemDto { OrderId = 1, ProductId = 1, Quantity= 2},

            };
            _mockOrderItemRepository.Setup(repo => repo.GetOrderItemAsync()).ReturnsAsync(orderItems.AsEnumerable());
            _mapper.Setup(mp => mp.Map<IEnumerable<OrderItemDto>>(orderItems)).Returns(orderItemsDto);
            var result = await _orderItemService.GetOrderItemAsync();
            Assert.NotNull(result);
            Assert.Equal(orderItems.Count, result.Count());
        }
        [Fact]
        public async Task GetOrderItemById_ReturnsNullWhenIdNotFound()
        {
            var orderId = 100;
            var productId = 100;
            _mockOrderItemRepository.Setup(repo => repo.GetOrderItemByIdAsync(orderId,productId)).ReturnsAsync((OrderItem)null);
            var result = await _orderItemService.GetOrderItemByIdAsync(orderId,productId);
            Assert.Null(result);
        }
        [Fact]
        public async Task GetOrderItemById_ReturnsOrder()
        {
            var orderId = 1;
            var productId = 1;
            var expectedOrderItem = new OrderItem { OrderId = 1, ProductId = 1, Quantity = 2 };
            var expectedOrderItemDto = new OrderItemDto { OrderId = 1, ProductId = 1, Quantity = 2 };
            _mockOrderItemRepository.Setup(repo => repo.GetOrderItemByIdAsync(orderId,productId)).ReturnsAsync(expectedOrderItem);
            _mapper.Setup(mp => mp.Map<OrderItemDto>(expectedOrderItem)).Returns(expectedOrderItemDto);
            var result = await _orderItemService.GetOrderItemByIdAsync(orderId,productId);
            Assert.NotNull(result);
            Assert.Equal(expectedOrderItem.OrderId, result.OrderId);
            Assert.Equal(expectedOrderItem.ProductId, result.ProductId);
            Assert.Equal(expectedOrderItem.Quantity, result.Quantity);
        }
        [Fact]
        public async Task CreateOrderItemWithValidData_CallRepository()
        {
            var orderItemDto = new OrderItemDto { OrderId = 1, ProductId = 1, Quantity = 2 };
            var orderItem = new OrderItem { OrderId = 1, ProductId = 1, Quantity = 2 };
            _mapper.Setup(m => m.Map<OrderItem>(It.IsAny<OrderItemDto>())).Returns(orderItem);

            _mockOrderItemRepository.Setup(repo => repo.CreateOrderItemAsync(It.IsAny<OrderItem>()))
                .Returns(Task.CompletedTask);
            _mockOrderItemRepository.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _orderItemService.CreateOrderItemAsync(orderItemDto);

            _mockOrderItemRepository.Verify(repo => repo.CreateOrderItemAsync(It.Is<OrderItem>(u =>
                u.OrderId == orderItem.OrderId &&
                u.ProductId == orderItem.ProductId &&
                u.Quantity == orderItem.Quantity)), Times.Once);
        }
    }
}
