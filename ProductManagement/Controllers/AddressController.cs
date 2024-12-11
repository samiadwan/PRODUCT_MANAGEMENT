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
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<AddressDTO> _validator;
        private readonly IMapper _mapper;

        public AddressController(ApplicationDbContext context, IMapper mapper, IValidator<AddressDTO> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _context.Addresses.Include(a => a.User).ToListAsync();
            var addressDtos = _mapper.Map<List<AddressDTO>>(addresses);
            return Ok(addressDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _context.Addresses.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
            if (address == null)
            {
                return NotFound();
            }
            var addressDto = _mapper.Map<AddressDTO>(address);
            return Ok(addressDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO addressDto)
        {
            var validationResult = _validator.Validate(addressDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var address = _mapper.Map<Address>(addressDto);
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var createdAddressDto = _mapper.Map<AddressDTO>(address);
            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, createdAddressDto);
        }
    }
}
