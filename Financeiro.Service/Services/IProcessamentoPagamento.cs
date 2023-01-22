using Financeiro.Service.Enums;
using Financeiro.Service.Models;

namespace Financeiro.Service.Services
{
    public interface IProcessamentoPagamento
    {
        decimal CalcularMulta(decimal valorDebito, decimal porcentagemVencimentoMulta);
        decimal CalcularMultaJuros(decimal valorDebito, decimal PorcentagemVencimentoMoraDia, int diasVencimento);
        DebitoSituacao CalcularSituacao(decimal valorPago, decimal valorPagar);
        int DiasVencimento(DateTime dataPagamento, DateTime dataVencimento);
        DebitoPagamento Processar(PagamentoProcessar pagamentoProcessar);
        decimal CalcularValorPagar(decimal valorOriginal, decimal valorMulta, decimal valorJuros);
    }
}