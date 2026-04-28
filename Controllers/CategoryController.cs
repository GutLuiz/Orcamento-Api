using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamento.Data;
using Orcamento.Models;
using System.Security.Claims;

namespace Orcamento.Controllers
{
    // um endpoint que somente pessoas autorizadas podem acessar (criar categorias)
    [ApiController]
    [Route("categories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        // aqui eu chamo meu banco e seu contexto, ond tem os moldais para buscar as "tabelas".
        private readonly AppDbContext _context;
        

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            // pega o user id do token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // fala que userid de categoria e igual a o userid do token
            category.UserId = userId;

            // adiciono categoria e salvo.
            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok(category);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // pega o userid do token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // mostra as categorias adicionadas desse cliente fazendoo uma validacao de userId
            var categories = _context.Categories
                .Where(c => c.UserId == userId)
                .ToList();

            return Ok(categories);
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            // instacia o id do corpo de categoria e fala que o parametro tem o mesmo valor
            var category = new Category { Id = id };

            // acha esse id no banco e remove dps salva
            _context.Categories.Attach(category);
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Category updatedCategory)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = updatedCategory.Name;

            _context.SaveChanges();

            return NoContent();
        }
    }
}