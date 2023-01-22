namespace Financeiro.Service.Models
{
    public class Debito
    {
        public int Id { get; set; }
        public decimal ValorOriginal { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
