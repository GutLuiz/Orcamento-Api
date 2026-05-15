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
            // pega o id do usuario do token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // valida se o id da ctegoria e o mesmo id da categoria de trasancao
            // e se o id do user na categoria  e o mesmo do token
            var category = _context.Categories
                .FirstOrDefault(c => c.Id == transaction.CategoryId && c.UserId == userId);

            if (category == null)
            {
                return BadRequest("Categoria inválida");
            }
                
            // fala que o usaurio do trasaction e o mesmo do usuario do token
            transaction.UserId = userId;

            // adiciona e salva
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(new { transaction.Id });
        }

        [HttpGet]
        public IActionResult GetAll(int? mes = null, int? ano = null)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var query = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);

            // filtro por mês
            if (mes.HasValue && ano.HasValue)
            {
                query = query.Where(t =>
                    t.Date.Month == mes.Value &&
                    t.Date.Year == ano.Value);
            }

            var transactions = query
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Amount,
                    t.Type,
                    t.Date,
                    t.CategoryId,
                    CategoryName = t.Category!.Name
                })
                .ToList();

            return Ok(transactions);
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            // falo que o id que eu passo como parametro e o mesmo do id da trasacao
            var trasaction = new Transaction { Id = id };

            // busco esse id na tabela especificada
            _context.Transactions.Attach(trasaction);
            // removo esse id
            _context.Transactions.Remove(trasaction);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Transaction updatedTransaction)
        {
            // pega o id do token
            var userId = int.Parse(
               User.FindFirst(ClaimTypes.NameIdentifier)!.Value
             );

            // procura transação do usuário
            var transaction = _context.Transactions
              .FirstOrDefault(t =>
                  t.Id == id &&
                  t.UserId == userId
              );
            if (transaction == null)
            {
                return NotFound();
            }

            // valida categoria
            var category = _context.Categories
                .FirstOrDefault(c =>
                    c.Id == updatedTransaction.CategoryId &&
                    c.UserId == userId
                );

            if (category == null)
            {
                return BadRequest("Categoria inválida");
            }

            // atualiza
            transaction.Title = updatedTransaction.Title;
            transaction.Amount = updatedTransaction.Amount;
            transaction.Type = updatedTransaction.Type;
            transaction.Date = updatedTransaction.Date;
            transaction.CategoryId = updatedTransaction.CategoryId;

            _context.SaveChanges();

            return NoContent();
        }
    }
}
