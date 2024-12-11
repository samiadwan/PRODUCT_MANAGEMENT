using AutoMapper;
using DataAccessLayer.AccessLayer;
using DataAccessLayer.AccessLayer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DTOs;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<UserDto> _validator;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, IMapper mapper, IValidator<UserDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users
                                .Include(u => u.Address)  
                                .Include(u => u.Orders)   
                                .ToList();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }


        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users
                               .Include(u => u.Address)  
                               .Include(u => u.Orders)  
                               .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            var validationResult = _validator.Validate(userDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var user = _mapper.Map<User>(userDto);
            _context.Users.Add(user);
            _context.SaveChanges();
            var createdUserDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, createdUserDto);
        }

    }
}
