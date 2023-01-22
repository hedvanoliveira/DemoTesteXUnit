using Financeiro.Service.Enums;
using Financeiro.Service.Models;
using Financeiro.Service.Services;
using Xunit;

namespace Financeiro.Test.Services
{
    public class ProcessamentoPagamentoTests
    {
        [Fact()]
        public void ProcessamentoPagamentoTest()
        {

        }

        [Fact()]
        public void ProcessarTest()
        {
            Configuracao configuracao = new()
            {
                PorcentagemVencimentoMulta = 2,
                PorcentagemVencimentoMoraDia = 0.0333333333333333M,
                ValorMaximoPermitidoPagamentoMenor = 0
            };

            PagamentoProcessar pagamentoProcessar = new()
            {
                NossoNumero = "123456",
                Valor = 107.73M,
                ValorPagamento = 110.32M,
                DataPagamento = DateTime.Parse("2010-12-13"),
                DataVencimento = DateTime.Parse("2010-12-01"),
            };

            ProcessamentoPagamento processamentoPagamento = new(configuracao);

            var debitoPagamento = processamentoPagamento.Processar(pagamentoProcessar);

            Assert.Equal(DebitoSituacao.Pago, debitoPagamento.DebitoSituacao);
        }

        [Fact()]
        public void DiasVencimentoTest()
        {
            Configuracao configuracao = new()
            {
                PorcentagemVencimentoMulta = 2,
                PorcentagemVencimentoMoraDia = 0.03M,
                ValorMaximoPermitidoPagamentoMenor = 0
            };

            PagamentoProcessar pagamentoProcessar = new()
            {
                NossoNumero = "123456",
                Valor = 107.73M,
                ValorPagamento = 107.73M,
                DataPagamento = DateTime.Parse("2012-12-13"),
                DataVencimento = DateTime.Parse("2012-12-01"),
            };

            ProcessamentoPagamento processamentoPagamento = new(configuracao);

            int diasVencimento = processamentoPagamento.DiasVencimento(pagamentoProcessar.DataPagamento, pagamentoProcessar.DataVencimento);

            Assert.Equal(12, diasVencimento);
        }

        [Fact()]
        public void CalcularMultaTest()
        {
            Configuracao configuracao = new()
            {
                PorcentagemVencimentoMulta = 2,
                PorcentagemVencimentoMoraDia = 0.03M,
                ValorMaximoPermitidoPagamentoMenor = 0
            };

            PagamentoProcessar pagamentoProcessar = new()
            {
                NossoNumero = "123456",
                Valor = 107.73M,
                ValorPagamento = 107.73M,
                DataPagamento = DateTime.Parse("2012-12-13"),
                DataVencimento = DateTime.Parse("2012-12-01"),
            };

            ProcessamentoPagamento processamentoPagamento = new ProcessamentoPagamento(configuracao);

            decimal valorMulta = processamentoPagamento.CalcularMulta(pagamentoProcessar.Valor, configuracao.PorcentagemVencimentoMulta);

            Assert.Equal(2.1546M, valorMulta);
        }

        [Fact()]
        public void CalcularMultaJurosTest()
        {
            Configuracao configuracao = new()
            {
                PorcentagemVencimentoMulta = 2,
                PorcentagemVencimentoMoraDia = 0.0333333333333333M,
                ValorMaximoPermitidoPagamentoMenor = 0
            };

            PagamentoProcessar pagamentoProcessar = new()
            {
                NossoNumero = "123456",
                Valor = 107.73M,
                ValorPagamento = 107.73M,
                DataPagamento = DateTime.Parse("2012-12-13"),
                DataVencimento = DateTime.Parse("2012-12-01"),
            };

            ProcessamentoPagamento processamentoPagamento = new(configuracao);

            decimal valorMultaJuros = processamentoPagamento.CalcularMultaJuros(pagamentoProcessar.Valor, configuracao.PorcentagemVencimentoMoraDia, 12);

            Assert.Equal(0.43092M, valorMultaJuros);
        }

        [Fact()]
        public void CalcularValorPagarTest()
        {
            Configuracao configuracao = new()
            {
                PorcentagemVencimentoMulta = 2,
                PorcentagemVencimentoMoraDia = 0.0333333333333333M,
                ValorMaximoPermitidoPagamentoMenor = 0
            };

            PagamentoProcessar pagamentoProcessar = new()
            {
                NossoNumero = "123456",
                Valor = 107.73M,
                ValorPagamento = 107.73M,
                DataPagamento = DateTime.Parse("2010-12-13"),
                DataVencimento = DateTime.Parse("2010-12-01"),
            };

            ProcessamentoPagamento processamentoPagamento = new(configuracao);

            decimal valorPagar = processamentoPagamento.CalcularValorPagar(pagamentoProcessar.Valor, 2.1546M, 0.43092M);

            Assert.Equal(110.32M, valorPagar);
        }

        [Theory]
        [InlineData(107.73, 107.73, DebitoSituacao.Pago)]
        [InlineData(100.73, 107.73, DebitoSituacao.PagoMenor)]
        [InlineData(110.73, 107.73, DebitoSituacao.PagoMaior)]
        [InlineData(0, 0, DebitoSituacao.NaoPago)]
        public void CalcularSituacaoTest(decimal valorPagamento, decimal valorPagar, DebitoSituacao debitoSituacao)
        {
            Configuracao configuracao = new()
            {
                PorcentagemVencimentoMulta = 2,
                PorcentagemVencimentoMoraDia = 0.0333333333333333M,
                ValorMaximoPermitidoPagamentoMenor = 0
            };

            ProcessamentoPagamento processamentoPagamento = new(configuracao);

            var situacao = processamentoPagamento.CalcularSituacao(valorPagamento, valorPagar);

            Assert.Equal(debitoSituacao, situacao);
        }

        [Theory]
        [InlineData(100.00, 100.00, DebitoSituacao.Pago, 10)]
        [InlineData(90.00, 100.00, DebitoSituacao.Pago, 10)]
        [InlineData(89.00, 100.00, DebitoSituacao.PagoMenor, 10)]
        public void CalcularSituacaoPagamentoMenorPermitidoTest(decimal valorPagamento, decimal valorPagar, DebitoSituacao debitoSituacao, decimal valorMaximoPagamentoMenor)
        {
            Configuracao configuracao = new()
            {
                PorcentagemVencimentoMulta = 0,
                PorcentagemVencimentoMoraDia = 0,
                ValorMaximoPermitidoPagamentoMenor = valorMaximoPagamentoMenor
            };

            ProcessamentoPagamento processamentoPagamento = new(configuracao);

            var situacao = processamentoPagamento.CalcularSituacao(valorPagamento, valorPagar);

            Assert.Equal(debitoSituacao, situacao);
        }
    }
}