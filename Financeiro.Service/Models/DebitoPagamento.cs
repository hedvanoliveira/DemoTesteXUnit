using Financeiro.Service.Enums;

namespace Financeiro.Service.Models
{
    public class DebitoPagamento
    {
        public int Id { get; set; }
        public int DebitoId { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataProcessamento { get; set; }
        public DebitoSituacao DebitoSituacao { get; set; }
    }
}
