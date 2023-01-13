using Microsoft.EntityFrameworkCore;
using NerdStore.Core.DomainObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace NerdStore.Catalogo.Domain
{
    [Owned]
    public class Dimensoes
    {
        [Column("altura")]
        public decimal Altura { get; private set; }
        [Column("largura")]
        public decimal Largura { get; private set; }
        [Column("profundidade")]
        public decimal Profundidade { get; private set; }

        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;

            Validacoes.ValidarSeMenorQue(Altura, 1, "O campo Altura não pode ser menor ou igual a 0");
            Validacoes.ValidarSeMenorQue(Largura, 1, "O campo Largura não pode ser menor ou igual a 0");
            Validacoes.ValidarSeMenorQue(Profundidade, 1, "O campo Profundidade não pode ser menor ou igual a 0");
        }

        public string DescricaoFormatada()
        {
            return $"LxAxP: {Largura} x {Altura} x {Profundidade}";
        }

        public override string? ToString()
        {
            return DescricaoFormatada();
        }
    }
}
