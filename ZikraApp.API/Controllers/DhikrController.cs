using Microsoft.AspNetCore.Mvc;
using ZikraApp.Application.Models;
using ZikraApp.Core.Interfaces;
using AutoMapper;

namespace ZikraApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DhikrController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DhikrController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dhikrs = await _unitOfWork.Repository<ZikraApp.Core.Entities.Dhikr>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<DhikrDto>>(dhikrs);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dhikr = await _unitOfWork.Repository<ZikraApp.Core.Entities.Dhikr>().GetByIdAsync(id);
            if (dhikr == null)
                return NotFound();
            var result = _mapper.Map<DhikrDto>(dhikr);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDhikrDto dto)
        {
            var dhikr = _mapper.Map<ZikraApp.Core.Entities.Dhikr>(dto);
            dhikr.CreatedAt = DateTime.UtcNow;
            var created = await _unitOfWork.Repository<ZikraApp.Core.Entities.Dhikr>().AddAsync(dhikr);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<DhikrDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDhikrDto dto)
        {
            var dhikr = await _unitOfWork.Repository<ZikraApp.Core.Entities.Dhikr>().GetByIdAsync(id);
            if (dhikr == null)
                return NotFound();
            _mapper.Map(dto, dhikr);
            await _unitOfWork.Repository<ZikraApp.Core.Entities.Dhikr>().UpdateAsync(dhikr);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dhikr = await _unitOfWork.Repository<ZikraApp.Core.Entities.Dhikr>().GetByIdAsync(id);
            if (dhikr == null)
                return NotFound();
            await _unitOfWork.Repository<ZikraApp.Core.Entities.Dhikr>().DeleteAsync(dhikr);
            return NoContent();
        }
    }
} 