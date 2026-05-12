using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orcamento.Data;
using Orcamento.Dtos;
using Orcamento.Models;

namespace Orcamento.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;

        private readonly AppDbContext _context;

        public AuthService(TokenService tokenService, AppDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<User?> RegistrarUsuario(RegisterDto dto)
        {
            var usuarioExiste = await _context.Users
                .FirstOrDefaultAsync(t => t.Email == dto.Email);

            if (usuarioExiste != null)
            {
                return null;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email.Trim().ToLower(),
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

    }
}
