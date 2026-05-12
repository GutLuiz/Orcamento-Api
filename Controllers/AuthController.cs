using Microsoft.AspNetCore.Mvc;
using Orcamento.Data;
using Orcamento.Models;
using BCrypt.Net;
using Orcamento.Services;
using Orcamento.Dtos;

namespace Orcamento.Controllers
{
     [ApiController]
     [Route("auth")]
     public class AuthController : ControllerBase
     {
        private readonly TokenService _tokenService;

        private readonly AppDbContext _context;

        private readonly AuthService _authService;

        public AuthController(TokenService tokenService, AppDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var usuario = await _authService.RegistrarUsuario(dto);

            if (usuario == null)
            {
                return BadRequest("Usuário já existe.");
            }

            return Ok("Usuário criado com sucesso.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return Unauthorized("Credenciais inválidas");
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!validPassword)
            {
                return Unauthorized("Credenciais inválidas");
            }
                
            var token = _tokenService.GenerateToken(user);

            return Ok(new
            {
                token = token
            });
        }
    }
}
