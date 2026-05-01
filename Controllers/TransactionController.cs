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

            return Ok(transaction);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // pega o id do usuario do token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // busca apenas as transacoes com esse determinado id
            var transactions = _context.Transactions
              .Where(t => t.UserId == userId)
              .Include(t => t.Category)
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
        public IActionResult Update(int id, [FromBody] Transaction updateTransaction)
        {
            // achar o id que eu passei no parametro
            var trasaction = _context.Transactions.Find(id);

            if(trasaction == null)
            {
                return NotFound();
            }
            
            trasaction.Title = updateTransaction.Title;
            trasaction.Amount = updateTransaction.Amount;
            trasaction.Type = updateTransaction.Type;
            trasaction.Date = updateTransaction.Date;
            trasaction.CategoryId = updateTransaction.CategoryId;

            if(trasaction.CategoryId == null)
            {
                return NotFound();
            }

            trasaction.UserId = updateTransaction.UserId;

            if(trasaction.UserId == null)
            {
                NotFound();
            }

            _context.SaveChanges();

            return NoContent();
        }
    }
}
