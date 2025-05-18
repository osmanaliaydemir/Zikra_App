using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZikraApp.Application.Models;
using ZikraApp.Core.Entities;
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
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur.
        /// </summary>
        /// <param name="dto">Kullanıcı kayıt bilgileri</param>
        /// <returns>Kayıtlı kullanıcı bilgisi</returns>
        /// <response code="200">Kullanıcı başarıyla kaydedildi</response>
        /// <response code="400">Email zaten kayıtlı</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            // Basit örnek: email unique kontrolü
            var exists = (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == dto.Email)).Any();
            if (exists)
                return BadRequest("Email already exists.");

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            var created = await _unitOfWork.Repository<User>().AddAsync(user);
            return Ok(_mapper.Map<UserDto>(created));
        }

        /// <summary>
        /// Kullanıcı girişi yapar ve JWT token döner.
        /// </summary>
        /// <param name="dto">Giriş bilgileri</param>
        /// <returns>JWT token</returns>
        /// <response code="200">Başarılı giriş</response>
        /// <response code="401">Geçersiz bilgiler</response>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);
            // Refresh token üretimi ve kaydı burada yapılabilir
            return Ok(new { Token = token });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            // Refresh token doğrulama ve yeni JWT üretimi burada yapılmalı
            return StatusCode(501, "Refresh token endpoint not implemented.");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
                return NotFound();
            var result = _mapper.Map<UserDto>(user);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            var created = await _unitOfWork.Repository<User>().AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<UserDto>(created));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
                return NotFound();
            _mapper.Map(dto, user);
            await _unitOfWork.Repository<User>().UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
            if (user == null)
                return NotFound();
            await _unitOfWork.Repository<User>().DeleteAsync(user);
            return NoContent();
        }

        /// <summary>
        /// Şifre sıfırlama bağlantısı gönderir.
        /// </summary>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null)
                return Ok(); // Güvenlik için her zaman OK dön

            // Token üret (örnek, gerçek projede DB'ye kaydedilmeli)
            var token = Guid.NewGuid().ToString();
            // Email gönder (mock)
            await _emailService.SendWelcomeEmailAsync(user.Email, $"Şifre sıfırlama linkiniz: https://frontend/reset-password?token={token}");
            return Ok();
        }

        /// <summary>
        /// Şifreyi sıfırlar.
        /// </summary>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            // Token doğrulama ve kullanıcı bulma (örnek, gerçek projede DB'den kontrol edilmeli)
            // Şimdilik mock
            var user = (await _unitOfWork.Repository<User>().GetAllAsync()).FirstOrDefault();
            if (user == null)
                return BadRequest("Invalid token.");
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _unitOfWork.Repository<User>().UpdateAsync(user);
            return Ok();
        }

        /// <summary>
        /// Email doğrulama bağlantısı gönderir.
        /// </summary>
        [HttpPost("send-verification-email")]
        [Authorize]
        public async Task<IActionResult> SendVerificationEmail()
        {
            // Kullanıcıyı token ile eşleştir (örnek, gerçek projede DB'ye kaydedilmeli)
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(Guid.Parse(userId));
            if (user == null)
                return Unauthorized();
            var token = Guid.NewGuid().ToString();
            await _emailService.SendWelcomeEmailAsync(user.Email, $"Email doğrulama linkiniz: https://frontend/verify-email?token={token}");
            return Ok();
        }

        /// <summary>
        /// Email doğrulamasını tamamlar.
        /// </summary>
        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDto dto)
        {
            // Token doğrulama ve kullanıcı bulma (örnek, gerçek projede DB'den kontrol edilmeli)
            // Şimdilik mock
            var user = (await _unitOfWork.Repository<User>().GetAllAsync()).FirstOrDefault();
            if (user == null)
                return BadRequest("Invalid token.");
            user.IsActive = true;
            await _unitOfWork.Repository<User>().UpdateAsync(user);
            return Ok();
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 