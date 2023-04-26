using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliverSystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Protect]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _unitOfWork.Users.GetAll();
            var data = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(new
            {
                data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var item = await _unitOfWork.Users.FindById(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Users.Add(user);
                await _unitOfWork.CompleteAsync();

                UserDto userDto = _mapper.Map<UserDto>(user);
                return CreatedAtAction("GetItem", new { user.Id }, userDto);
            }

            return new JsonResult("Somethign Went wrong") { StatusCode = 500 };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(string id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            await _unitOfWork.Users.Upsert(user);
            await _unitOfWork.CompleteAsync();

            // Following up the REST standart on update we need to return NoContent
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var item = await _unitOfWork.Users.FindById(id);

            if (item == null)
                return NotFound();

            await _unitOfWork.Users.Delete(id);
            await _unitOfWork.CompleteAsync();

            return Ok(item);
        }
    }
}
