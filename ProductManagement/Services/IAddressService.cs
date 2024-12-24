using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using ProductManagement.DTOs;
using ProductManagement.Repositories;

namespace ProductManagement.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAddressesAsync();
        Task<AddressDTO> GetAddressByIdAsync(int id);
        Task<AddressDTO> CreateAddressAsync(AddressDTO addressDto);

    }

    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddressDTO> _validator;
        public AddressService(IAddressRepository addressRepository, IMapper mapper, IValidator<AddressDTO> validator)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<IEnumerable<AddressDTO>> GetAddressesAsync()
        {
           var addresses =  await _addressRepository.GetAddressesAsync();
            return _mapper.Map<IEnumerable<AddressDTO>>(addresses);
        }
      
        public async Task<AddressDTO> GetAddressByIdAsync(int id)
        {
           var address = await _addressRepository.GetAddressByIdAsync(id);
            return _mapper.Map<AddressDTO>(address);
        }
        public async Task<AddressDTO> CreateAddressAsync(AddressDTO addressDto)
        {
            var validateAddressResult = _validator.Validate(addressDto);
            if (!validateAddressResult.IsValid)
            {
                throw new ValidationException(validateAddressResult.Errors);
            }
            var address = _mapper.Map<Address>(addressDto);
            await _addressRepository.CreateAddressAsync(address);
            await _addressRepository.SaveChangesAsync();
            return _mapper.Map<AddressDTO>(address);
        }

    }
}
