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
        public async Task<IActionResult> Login( LoginDto dto)
        {
            var token = await _authService.LoginUsuario(dto);

            if(token == null)
            {
                return Unauthorized("Email ou senha invalidos!");
            }
            return Ok(new { token = token });
        }
    }
}
