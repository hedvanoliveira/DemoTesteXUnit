namespace Financeiro.Service.Models
{
    public class DebitoEmissao
    {
        public int Id { get; set; }
        public int DebitoId { get; set; }
        public string NossoNumero { get; set; } = string.Empty;
        public decimal ValorOriginal { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}