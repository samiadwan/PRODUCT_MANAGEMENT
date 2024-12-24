using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using ProductManagement.DTOs;
using ProductManagement.Repositories;

namespace ProductManagement.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto productDto);      
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductDto> _validator;
        public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductDto> validator)
        {
            _productRepository = productRepository;
           _mapper = mapper;
           _validator = validator;
        }
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var validationResult = _validator.Validate(productDto);
            if (!validationResult.IsValid) 
            {
                throw new ValidationException(validationResult.Errors);
            }
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.CreateProductAsync(product);
            await _productRepository.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

    }
}
