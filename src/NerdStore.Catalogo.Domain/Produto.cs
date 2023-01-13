using NerdStore.Core.DomainObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NerdStore.Catalogo.Domain
{
    [Table("Produtos")]
    public class Produto : Entity, IAggregateRoot
    {
        public Guid CategoriaId { get; private set; }

        [Column("nome")]
        [MaxLength(250)]
        public string Nome { get; private set; }

        [Column("descricao")]
        [MaxLength(500)]
        public string Descricao { get; private set; }

        [Column("ativo")]
        public bool Ativo { get; private set; }

        [Column("valor")]
        public decimal Valor { get; private set; }

        [Column("dataCadastro")]
        public DateTime DataCadastro { get; private set; }

        [Column("imagem")]
        [MaxLength(250)]
        public string Imagem { get; private set; }

        [Column("quantidadeEstoque")]
        public int QuantidadeEstoque { get; private set; }

        [ForeignKey(nameof(CategoriaId))]
        public virtual Categoria Categoria { get; private set; }

        public Dimensoes Dimensoes { get; private set; }

        protected Produto() { }

        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid categoriaId, DateTime dataCadastro, string imagem, Dimensoes dimensoes)
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            CategoriaId = categoriaId;
            Dimensoes = dimensoes;

            Validar();
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string descricao)
        {
            Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente");
            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O campo Nome do produto não pode estar vazio");
            Validacoes.ValidarSeVazio(Descricao, "O campo Descricao do produto não pode estar vazio");
            Validacoes.ValidarSeIgual(CategoriaId, Guid.Empty, "O campo CategoriaId do produto não pode estar vazio");
            Validacoes.ValidarSeMenorQue(Valor, 1, "O campo Valor do produto não pode ser menor ou igual a zero");
            Validacoes.ValidarSeVazio(Imagem, "O campo Imagem do produto não pode estar vazio");
        }
    }
}
