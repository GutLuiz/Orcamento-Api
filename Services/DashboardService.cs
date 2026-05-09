using Microsoft.EntityFrameworkCore;
using Orcamento.Data;
using Orcamento.Dtos;
using Orcamento.Models;
using System.ComponentModel;

namespace Orcamento.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CardsDto> BuscarValoresCards(int userId)
        {
            var receita = await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionType.Income)
                .SumAsync(t => t.Amount);

            var despesa = await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionType.Expense)
                .SumAsync(t => t.Amount);

            return new CardsDto
            {
                receita = receita,
                despesa = despesa,
                saldoAtual = receita - despesa
            };
        }
        public async Task<List<GraficoDto>> BuscarValoresGrafico(int userId)
        {
            return await _context.Transactions.Where(
                t => t.UserId == userId).GroupBy(
                C => C.Category.Name).Select(g => new GraficoDto
                {
                    categoria = g.Key,
                    valor = g.Sum(t => t.Amount) 
                }).OrderByDescending(x => x.valor)
                .Take(5)
                .ToListAsync();
        }
        public async Task<List<ListaDto>> BuscarValoresLista(int userId)
        {
            return await _context.Transactions
                  .Where(t => t.UserId == userId)
                  .Select(g => new ListaDto
                  {
                      titulo = g.Title,
                      valor = g.Amount,
                      data = g.Date,     categoria = g.Category.Name
                  }).OrderByDescending(x => x.data)
                 
                      .Take(5)
                      .ToListAsync();
        }
    }
}
