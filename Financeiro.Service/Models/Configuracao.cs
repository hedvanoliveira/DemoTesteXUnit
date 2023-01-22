using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financeiro.Service.Models
{
    public class Configuracao
    {
        public decimal PorcentagemVencimentoMulta { get; set; }
        public decimal PorcentagemVencimentoMoraDia { get; set; }
        public decimal ValorMaximoPermitidoPagamentoMenor { get; set; }
    }
}
