using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
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
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mocProductRepositroy; 
        private readonly ProductService _productService;
        private readonly Mock<IMapper> _mapper;
        private readonly ProductValidator _validator;

        public ProductServiceTests()
        {
            _mocProductRepositroy = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();
            _validator = new ProductValidator();
            _productService = new ProductService(_mocProductRepositroy.Object, _mapper.Object, _validator);
        }
        [Fact]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Smart Watch", Price=5000},
                new Product { Id = 2, Name = "HeadPhone", Price=500},
            };
            var productsdto = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Smart Watch", Price=5000},
                new ProductDto { Id = 1, Name = "Smart Watch", Price=5000},
            };
            _mocProductRepositroy.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(products.AsEnumerable());
            _mapper.Setup(mp => mp.Map<IEnumerable<ProductDto>>(products)).Returns(productsdto);
            var result = await _productService.GetProductsAsync();
            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count());
        }
        [Fact]
        public async Task GetProductById_ReturnsNullWhenIdNotFound()
        {
            var productId = 100;
            _mocProductRepositroy.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync((Product)null);
            var result = await _productService.GetProductByIdAsync(productId);
            Assert.Null(result);
        }
        [Fact]
        public async Task GetProductById_ReturnsProduct()
        {
            var productId = 1;
            var expectedProduct = new Product { Id = 1, Name = "Smart Watch", Price = 5000 };
            var expectedProductDto = new ProductDto { Id = 1, Name = "Smart Watch", Price = 5000 };
            _mocProductRepositroy.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync(expectedProduct);
            _mapper.Setup(mp => mp.Map<ProductDto>(expectedProduct)).Returns(expectedProductDto);
            var result = await _productService.GetProductByIdAsync(productId);
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.Id, result.Id);
            Assert.Equal(expectedProduct.Name, result.Name);
            Assert.Equal(expectedProduct.Price, result.Price);
        }
        [Fact]
        public async Task CreateProductWithValidData_CallRepository()
        {
            var productDto = new ProductDto { Id = 1, Name = "Smart Watch", Price = 5000 }; 
            var product = new Product { Id = 1, Name = "Smart Watch", Price = 5000 };
            _mapper.Setup(m => m.Map<Product>(It.IsAny<ProductDto>()))
           .Returns(product);

            _mocProductRepositroy.Setup(repo => repo.CreateProductAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);
            _mocProductRepositroy.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _productService.CreateProductAsync(productDto);

            _mocProductRepositroy.Verify(repo => repo.CreateProductAsync(It.Is<Product>(u =>
                u.Id == product.Id &&
                u.Name == product.Name &&
                u.Price == product.Price)), Times.Once);
        }
    }
}
