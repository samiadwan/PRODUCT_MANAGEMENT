using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using Moq;
using ProductManagement.DTOs;
using ProductManagement.Repositories;
using ProductManagement.Services;
using ProductManagement.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.UnitTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly OrderService _orderService;
        private readonly Mock<IMapper> _mapper;
        private readonly OrderValidator _validator;

        public OrderServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _validator = new OrderValidator();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _orderService = new OrderService(_mockOrderRepository.Object, _mapper.Object, _validator);
        }
        [Fact]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, UserId = 1, OrderDate= new DateTime()},
                new Order { Id = 2, UserId = 2, OrderDate= new DateTime()},
                
            };
            var ordersDto = new List<OrderDto>
            {
                new OrderDto { Id = 1, UserId = 1, OrderDate= new DateTime(2024, 12, 29)},
                new OrderDto { Id = 2, UserId = 2, OrderDate= new DateTime(2024, 12, 29)},

            };
            _mockOrderRepository.Setup(repo => repo.GetOrdersAsync()).ReturnsAsync(orders.AsEnumerable());
            _mapper.Setup(mp => mp.Map<IEnumerable<OrderDto>>(orders)).Returns(ordersDto);
            var result = await _orderService.GetOrdersAsync();
            Assert.NotNull(result);
            Assert.Equal(orders.Count, result.Count());
        }
        [Fact]
        public async Task GetOrderById_ReturnsNullWhenIdNotFound()
        {
            var orderId = 100;
            _mockOrderRepository.Setup(repo => repo.GetOrderByIdAsync(orderId)).ReturnsAsync((Order)null);
            var result = await _orderService.GetOrderByIdAsync(orderId);
            Assert.Null(result);
        }
        [Fact]
        public async Task GetOrderById_ReturnsOrder()
        {
            var orderId = 1;
            var expectedOrder = new Order { Id = 1, UserId = 1, OrderDate = new DateTime(2024, 12, 29) };
            var expectedOrderDto = new OrderDto { Id = 1, UserId = 1, OrderDate = new DateTime(2024, 12, 29) };
            _mockOrderRepository.Setup(repo => repo.GetOrderByIdAsync(orderId)).ReturnsAsync(expectedOrder);
            _mapper.Setup(mp => mp.Map<OrderDto>(expectedOrder)).Returns(expectedOrderDto);
            var result = await _orderService.GetOrderByIdAsync(orderId);
            Assert.NotNull(result);
            Assert.Equal(expectedOrder.Id, result.Id);
            Assert.Equal(expectedOrder.UserId, result.UserId);
            Assert.Equal(expectedOrder.OrderDate, result.OrderDate);
        }
        [Fact]
        public async Task CreateOrderWithValidData_CallRepository()
        {
            var orderDto = new OrderDto { Id = 1, UserId = 1, OrderDate = new DateTime(2024, 12, 29) };
            var order = new Order { Id = 1, UserId = 1, OrderDate = new DateTime(2024, 12, 29) };
            _mapper.Setup(m => m.Map<Order>(It.IsAny<OrderDto>())) .Returns(order);

            _mockOrderRepository.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);
            _mockOrderRepository.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _orderService.CreateOrderAsync(orderDto);

            _mockOrderRepository.Verify(repo => repo.CreateOrderAsync(It.Is<Order>(u =>
                u.Id == order.Id &&
                u.UserId == order.UserId &&
                u.OrderDate == order.OrderDate)), Times.Once);
        }
    }
}
