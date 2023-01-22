using Financeiro.Service.Enums;
using System.Drawing;

namespace Financeiro.Service.Models
{
    public class PagamentoProcessar
    {
        public string NossoNumero { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}