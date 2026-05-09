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
        public string titulo { get; set; } = "";
        public decimal valor { get; set; } = 0;
        public DateTime data { get; set; }
        public string categoria { get; set; } = "";
    }
}
