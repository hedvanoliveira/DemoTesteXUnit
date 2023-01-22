using Financeiro.Service.Enums;
using Financeiro.Service.Models;

namespace Financeiro.Service.Services
{
    public class ProcessamentoPagamento : IProcessamentoPagamento
    {
        readonly decimal _porcentagemVencimentoMulta;
        readonly decimal _porcentagemVencimentoMoraDia;
        readonly decimal _valorMaximoPermitidoPagamentoMenor;

        public ProcessamentoPagamento(Configuracao configuracao)
        {
            _porcentagemVencimentoMulta = configuracao.PorcentagemVencimentoMulta;
            _porcentagemVencimentoMoraDia = configuracao.PorcentagemVencimentoMoraDia;
            _valorMaximoPermitidoPagamentoMenor = configuracao.ValorMaximoPermitidoPagamentoMenor;
        }

        public DebitoPagamento Processar(PagamentoProcessar pagamentoProcessar)
        {
            decimal valorMulta = 0;
            decimal valorJuros = 0;

            int qtdDiasVencimento = DiasVencimento(pagamentoProcessar.DataPagamento, pagamentoProcessar.DataVencimento);

            if (qtdDiasVencimento > 0)
            {
                valorMulta = CalcularMulta(pagamentoProcessar.Valor, _porcentagemVencimentoMulta);
                valorJuros = CalcularMultaJuros(pagamentoProcessar.Valor, _porcentagemVencimentoMoraDia, qtdDiasVencimento);
            }

            decimal valorPagar = CalcularValorPagar(pagamentoProcessar.Valor, valorMulta, valorJuros);
            var situacao = CalcularSituacao(valorPagar, pagamentoProcessar.ValorPagamento);

            DebitoPagamento debitoPagamento = new DebitoPagamento()
            {
                DataPagamento = pagamentoProcessar.DataPagamento,
                DataProcessamento = DateTime.Now,
                ValorOriginal = pagamentoProcessar.Valor,
                ValorPagamento = pagamentoProcessar.ValorPagamento,
                DebitoSituacao = situacao
            };

            return debitoPagamento;
        }

        public int DiasVencimento(DateTime dataPagamento, DateTime dataVencimento)
        {
            return (dataPagamento - dataVencimento).Days;
        }

        public decimal CalcularMulta(decimal valorDebito, decimal porcentagemVencimentoMulta)
        {
            decimal total = valorDebito * porcentagemVencimentoMulta / 100;
            return Math.Round(total, 5);
        }

        public decimal CalcularMultaJuros(decimal valorDebito, decimal PorcentagemVencimentoMoraDia, int diasVencimento)
        {
            decimal porcentagem = diasVencimento * PorcentagemVencimentoMoraDia;
            decimal total = valorDebito * porcentagem / 100;
            return Math.Round(total, 5);
        }

        public decimal CalcularValorPagar(decimal valorOriginal, decimal valorMulta, decimal valorJuros)
        {
            decimal total = valorOriginal + valorMulta + valorJuros;
            return Math.Round(total, 2);
        }

        public DebitoSituacao CalcularSituacao(decimal valorPago, decimal valorPagar)
        {
            if (valorPago <= 0)
            {
                return DebitoSituacao.NaoPago;
            }
            else if (valorPago == valorPagar)
            {
                return DebitoSituacao.Pago;
            }
            else if (valorPago > valorPagar)
            {
                return DebitoSituacao.PagoMaior;
            }
            else
            {
                decimal valorPagoPermitido = _valorMaximoPermitidoPagamentoMenor + valorPago;

                if (valorPagoPermitido >= valorPagar)
                {
                    return DebitoSituacao.Pago;
                }
                else
                {
                    return DebitoSituacao.PagoMenor;
                }
            }
        }

    }
}