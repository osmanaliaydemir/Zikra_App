using Microsoft.AspNetCore.Mvc;
using ZikraApp.Application.Models;
using ZikraApp.Core.Interfaces;
using AutoMapper;

namespace ZikraApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDhikrController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserDhikrController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserDhikrs(Guid userId)
        {
            var userDhikrs = await _unitOfWork.Repository<ZikraApp.Core.Entities.UserDhikr>()
                .FindAsync(ud => ud.UserId == userId);
            var result = _mapper.Map<IEnumerable<UserDhikrDto>>(userDhikrs);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] CreateUserDhikrDto dto)
        {
            var userDhikr = _mapper.Map<ZikraApp.Core.Entities.UserDhikr>(dto);
            userDhikr.CreatedAt = DateTime.UtcNow;
            userDhikr.IsActive = true;
            var created = await _unitOfWork.Repository<ZikraApp.Core.Entities.UserDhikr>().AddAsync(userDhikr);
            return CreatedAtAction(nameof(GetUserDhikrs), new { userId = created.UserId }, _mapper.Map<UserDhikrDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReminder(Guid id, [FromBody] UpdateUserDhikrDto dto)
        {
            var userDhikr = await _unitOfWork.Repository<ZikraApp.Core.Entities.UserDhikr>().GetByIdAsync(id);
            if (userDhikr == null)
                return NotFound();

            _mapper.Map(dto, userDhikr);
            userDhikr.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Repository<ZikraApp.Core.Entities.UserDhikr>().UpdateAsync(userDhikr);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unsubscribe(Guid id)
        {
            var userDhikr = await _unitOfWork.Repository<ZikraApp.Core.Entities.UserDhikr>().GetByIdAsync(id);
            if (userDhikr == null)
                return NotFound();

            userDhikr.IsActive = false;
            userDhikr.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Repository<ZikraApp.Core.Entities.UserDhikr>().UpdateAsync(userDhikr);
            return NoContent();
        }
    }
} 