using Microsoft.AspNetCore.Mvc;
using ZikraApp.Application.Models;
using ZikraApp.Core.Interfaces;
using AutoMapper;

namespace ZikraApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DhikrProgressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DhikrProgressController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProgress(Guid userId)
        {
            var progress = await _unitOfWork.Repository<ZikraApp.Core.Entities.DhikrProgress>()
                .FindAsync(p => p.UserId == userId);
            var result = _mapper.Map<IEnumerable<DhikrProgressDto>>(progress);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RecordProgress([FromBody] CreateDhikrProgressDto dto)
        {
            var progress = _mapper.Map<ZikraApp.Core.Entities.DhikrProgress>(dto);
            progress.CreatedAt = DateTime.UtcNow;
            var created = await _unitOfWork.Repository<ZikraApp.Core.Entities.DhikrProgress>().AddAsync(progress);
            return CreatedAtAction(nameof(GetUserProgress), new { userId = created.UserId }, _mapper.Map<DhikrProgressDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgress(Guid id, [FromBody] UpdateDhikrProgressDto dto)
        {
            var progress = await _unitOfWork.Repository<ZikraApp.Core.Entities.DhikrProgress>().GetByIdAsync(id);
            if (progress == null)
                return NotFound();

            _mapper.Map(dto, progress);
            progress.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Repository<ZikraApp.Core.Entities.DhikrProgress>().UpdateAsync(progress);
            return NoContent();
        }

        [HttpGet("user/{userId}/history")]
        public async Task<IActionResult> GetUserDhikrHistory(Guid userId)
        {
            var progress = await _unitOfWork.Repository<ZikraApp.Core.Entities.DhikrProgress>()
                .FindAsync(p => p.UserId == userId);
            var result = _mapper.Map<IEnumerable<DhikrProgressDto>>(progress);
            return Ok(result);
        }

        [HttpGet("user/{userId}/stats")]
        public async Task<IActionResult> GetUserDhikrStats(Guid userId)
        {
            var progress = await _unitOfWork.Repository<ZikraApp.Core.Entities.DhikrProgress>()
                .FindAsync(p => p.UserId == userId);
            var total = progress.Sum(p => p.Count);
            var most = progress.GroupBy(p => p.DhikrId)
                .OrderByDescending(g => g.Sum(x => x.Count))
                .Select(g => new { DhikrId = g.Key, Total = g.Sum(x => x.Count) })
                .FirstOrDefault();
            return Ok(new { TotalCount = total, MostRecited = most });
        }
    }
} 