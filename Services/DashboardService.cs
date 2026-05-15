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
        private readonly DateTime _inicioMesAtual;
        private readonly DateTime _hoje;

        public DashboardService(AppDbContext context)
        {
            _context = context;

            _inicioMesAtual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            _hoje = DateTime.Now;
        }
        
        public async Task<CardsDto> BuscarValoresCards(int userId)
        {
            var receita = await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionType.Income && 
                    t.Date >= _inicioMesAtual)
                .SumAsync(t => t.Amount);

            var despesa = await _context.Transactions
                .Where(t => t.UserId == userId && t.Type == TransactionType.Expense &&
                    t.Date >= _inicioMesAtual)
                .SumAsync(t => t.Amount);

            return new CardsDto
            {
                receita = receita,
                despesa = despesa,
                saldoAtual = receita - despesa
            };
        }
        public async Task<List<GraficoDto>> BuscarValoresGraficoDespesas(int userId)
        {
            return await _context.Transactions.Where(
                t => t.UserId == userId && t.Type == TransactionType.Expense &&
                    t.Date >= _inicioMesAtual).GroupBy(
                C => C.Category.Name).Select(g => new GraficoDto
                {
                    categoria = g.Key,
                    valor = g.Sum(t => t.Amount) 
                }).OrderByDescending(x => x.valor)
                .Take(5)
                .ToListAsync();
        }
        public async Task<List<GraficoDto>>BuscarValoresGraficoReceitas(int userId)
        {
            return await _context.Transactions.Where(
                t => t.UserId == userId && t.Type == TransactionType.Income &&
                    t.Date >= _inicioMesAtual).GroupBy(
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
                  .Where(t => t.UserId == userId &&
                    t.Date >= _inicioMesAtual)
                  .Select(g => new ListaDto
                  {
                      Title = g.Title,
                      amount = g.Amount,
                      date = g.Date, 
                      categoryName = g.Category.Name,
                  }).OrderByDescending(x => x.date)
                    .Take(8)
                    .ToListAsync();
        }
    }
}
