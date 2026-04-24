using Microsoft.AspNetCore.Mvc;
using Orcamento.Data;
using Orcamento.Models;
using BCrypt.Net;
using Orcamento.Services;

namespace Orcamento.Controllers
{
     [ApiController]
     [Route("auth")]
     public class AuthController : ControllerBase
     {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            var existingUser = InMemoryDb.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (existingUser != null)
            {
                return BadRequest("Usuário já existe");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            InMemoryDb.Users.Add(user);

            return Ok("Usuário criado");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = InMemoryDb.Users
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
                //usuario = user,
                token = token
            });
        }
    }
}
