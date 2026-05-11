namespace Orcamento.Dtos
{
    public class CardsDto
    {
        public decimal despesa { get; set; } = 0;
        public decimal receita { get; set; } = 0;
        public decimal saldoAtual { get; set; } = 0;
    }
    public class GraficoDto
    {
        public string categoria { get; set; } = "";
        public decimal valor { get; set; } = 0;
    }
    public class ListaDto
    {
        public string Title { get; set; } = "";
        public decimal amount { get; set; } = 0;
        public DateTime date { get; set; }
        public string categoryName { get; set; } = "";
        public string type { get; set; } = "";
    }
}
