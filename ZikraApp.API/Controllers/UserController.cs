using Microsoft.AspNetCore.Mvc;
using ZikraApp.Application.Models;
using ZikraApp.Core.Interfaces;
using AutoMapper;

namespace ZikraApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.Repository<ZikraApp.Core.Entities.User>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _unitOfWork.Repository<ZikraApp.Core.Entities.User>().GetByIdAsync(id);
            if (user == null)
                return NotFound();
            var result = _mapper.Map<UserDto>(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var user = _mapper.Map<ZikraApp.Core.Entities.User>(dto);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            var created = await _unitOfWork.Repository<ZikraApp.Core.Entities.User>().AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<UserDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _unitOfWork.Repository<ZikraApp.Core.Entities.User>().GetByIdAsync(id);
            if (user == null)
                return NotFound();
            _mapper.Map(dto, user);
            await _unitOfWork.Repository<ZikraApp.Core.Entities.User>().UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _unitOfWork.Repository<ZikraApp.Core.Entities.User>().GetByIdAsync(id);
            if (user == null)
                return NotFound();
            await _unitOfWork.Repository<ZikraApp.Core.Entities.User>().DeleteAsync(user);
            return NoContent();
        }
    }
} 