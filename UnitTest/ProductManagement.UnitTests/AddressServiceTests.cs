using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using Moq;
using NSubstitute;
using ProductManagement.DTOs;
using ProductManagement.Repositories;
using ProductManagement.Services;
using ProductManagement.Validators;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.UnitTests
{
    public class AddressServiceTests
    {
        private readonly Mock<IAddressRepository> _mockAddressRepository;
        private readonly AddressService _addressService;
        private readonly Mock<IMapper> _mapper;
        private readonly AddressValidator _validator;

        public AddressServiceTests()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
            _mapper = new Mock<IMapper>();
            _validator = new AddressValidator();
            _addressService = new AddressService(_mockAddressRepository.Object, _mapper.Object, _validator);
        }

        [Fact]
        public async Task GetAllAddressesAsync_ReturnsAllAddresses()
        {
            var addresses = new List<Address>
            {
                new Address { Id = 1, Street = "123 Main St", PostalCode = "1234", City="Springfield"},
                new Address { Id = 2, Street = "456 Elm St", PostalCode = "5678", City="Shelbyville"},
               
            };
            var addressesdto = new List<AddressDTO>
            {
                new AddressDTO { Id = 1, Street = "123 Main St", PostalCode = "1234", City="Springfield"},
                new AddressDTO { Id = 2, Street = "456 Elm St", PostalCode = "5678", City="Shelbyville"},

            };
            _mockAddressRepository.Setup(repo => repo.GetAddressesAsync()).ReturnsAsync(addresses.AsEnumerable());
            _mapper.Setup(mp => mp.Map<IEnumerable<AddressDTO>>(addresses)).Returns(addressesdto);
            var result = await _addressService.GetAddressesAsync();
            Assert.NotNull(result);
            Assert.Equal(addresses.Count, result.Count());
        }
        [Fact]
        public async Task GetAddressById_ReturnsNullWhenIdNotFound()
        {
            var addressId = 100;
            _mockAddressRepository.Setup(repo => repo.GetAddressByIdAsync(addressId)).ReturnsAsync((Address)null);
            var result = await _addressService.GetAddressByIdAsync(addressId);
            Assert.Null(result);
        }
        [Fact]
        public async Task GetAddressById_ReturnsAddress()
        {
            var addressId = 1;
            var expectedAddress = new Address { Id = 1, Street = "123 Main St", PostalCode = "1234", City = "Springfield" };
            var expectedAddressDto = new AddressDTO { Id = 1, Street = "123 Main St", PostalCode = "1234", City = "Springfield" };
            _mockAddressRepository.Setup(repo => repo.GetAddressByIdAsync(addressId)).ReturnsAsync(expectedAddress);
            _mapper.Setup(mp => mp.Map<AddressDTO>(expectedAddress)).Returns(expectedAddressDto);
            var result = await _addressService.GetAddressByIdAsync(addressId);
            Assert.NotNull(result);
            Assert.Equal(expectedAddress.Id, result.Id);
            Assert.Equal(expectedAddress.Street, result.Street);
            Assert.Equal(expectedAddress.PostalCode, result.PostalCode);
            Assert.Equal(expectedAddress.City, result.City);
        }
        [Fact]
        public async Task CreateAddressWithValidData_CallRepository()
        {
            var addressDto = new AddressDTO { Id = 10, Street = "123 Main St", PostalCode = "1234", City = "Springfield" };
            var address = new Address { Id = 10, Street = "123 Main St", PostalCode = "1234", City = "Springfield" };
            _mapper.Setup(m => m.Map<Address>(It.IsAny<AddressDTO>()))
           .Returns(address);

            _mockAddressRepository.Setup(repo => repo.CreateAddressAsync(It.IsAny<Address>()))
                .Returns(Task.CompletedTask);
            _mockAddressRepository.Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _addressService.CreateAddressAsync(addressDto);

            _mockAddressRepository.Verify(repo => repo.CreateAddressAsync(It.Is<Address>(u =>
                u.Id == address.Id &&
                u.Street == address.Street &&
                u.PostalCode == address.PostalCode &&
                u.City == address.City)), Times.Once);
        }
    }
}
