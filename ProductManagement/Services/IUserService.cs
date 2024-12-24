using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using ProductManagement.DTOs;
using ProductManagement.Repositories;

namespace ProductManagement.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(UserDto userDto);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDto> _validator;
        public UserService(IUserRepository userRepository, IMapper mapper, IValidator<UserDto> validator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var user = await _userRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(user);                 
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
           var userById = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDto>(userById);
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            var validationResult = _validator.Validate(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var user = _mapper.Map<User>(userDto);
            await _userRepository.CreateUserAsync(user);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }
    }
}
