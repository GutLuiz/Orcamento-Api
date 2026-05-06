using Microsoft.EntityFrameworkCore;
using Orcamento.Data;
using Orcamento.Dtos;
using Orcamento.Models;

namespace Orcamento.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CardsDto> BuscarValores(int userId)
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
    }
}
