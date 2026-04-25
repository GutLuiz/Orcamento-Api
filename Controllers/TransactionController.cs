using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orcamento.Data;
using Orcamento.Models;
using System.Security.Claims;

namespace Orcamento.Controllers
{
    [ApiController]
    [Route("transactions")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Transaction transaction)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // valida se a categoria pertence ao usuário
            var category = _context.Categories
                .FirstOrDefault(c => c.Id == transaction.CategoryId && c.UserId == userId);

            if (category == null)
                return BadRequest("Categoria inválida");

            transaction.UserId = userId;

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(transaction);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var transactions = _context.Transactions
              .Where(t => t.UserId == userId)
              .Include(t => t.Category)
              .ToList();

            return Ok(transactions);
        }
    }
}
