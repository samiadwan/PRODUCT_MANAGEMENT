using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductManagement.DTOs;
using ProductManagement.Repositories;
using ProductManagement.Services;
using ProductManagement.Validators;


namespace ProductManagement.UnitTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        private readonly Mock<IMapper> _mapper;
        private readonly UserValidator _validator;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();
            _validator = new UserValidator();
            _userService = new UserService(_mockUserRepository.Object, _mapper.Object, _validator);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "Andrew", Email = "andrew@gmail.com" },
                new User { Id = 2, Name = "Jasica", Email = "Jasica@gmail.com" },

            };
            var usersDto = new List<UserDto>
            {
                new UserDto { Id = 1, Name = "Andrew", Email = "andrew@gmail.com" },
                new UserDto { Id = 2, Name = "Jasica", Email = "Jasica@gmail.com" },

            };
            _mockUserRepository.Setup(repo => repo.GetUsersAsync()).ReturnsAsync(users.AsEnumerable());
            _mapper.Setup(mp => mp.Map<IEnumerable<UserDto>>(users)).Returns(usersDto);
            var result = await _userService.GetUsersAsync();
            Assert.NotNull(result);
            Assert.Equal(users.Count,result.Count());
        }
        [Fact]
        public async Task GetUserById_ReturnsNullWhenIdNotFound()
        {
            var userId = 100;
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
            var result = await _userService.GetUserByIdAsync(userId);
            Assert.Null(result);
        }
        [Fact]
        public async Task GetUserById_ReturnsUser()
        {
            var userId = 1;
            var expectedUser =new User { Id = 1, Name = "Andrew", Email = "andrew@gmail.com" };
            var expectedUserDto =new UserDto { Id = 1, Name = "Andrew", Email = "andrew@gmail.com" };
            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);
            _mapper.Setup(mp => mp.Map<UserDto>(expectedUser)).Returns(expectedUserDto);
            var result = await _userService.GetUserByIdAsync(userId);
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Email, result.Email);

        }
        [Fact]
        public async Task CreateUserWithValidData_CallRepository()
        {
            var userDto = new UserDto { Id = 10, Name = "Andrew dev", Email = "andrew@gmail.com"};
            var user = new User { Id = 10, Name = "Andrew dev", Email = "andrew@gmail.com"};
            _mapper.Setup(mp => mp.Map<User>(userDto)).Returns(user);
            _mockUserRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUserRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.CreateUserAsync(userDto);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.Is<User>(u =>
            u.Id == user.Id &&
            u.Name == user.Name &&
            u.Email == user.Email)), Times.Once);

        }

    }
}
