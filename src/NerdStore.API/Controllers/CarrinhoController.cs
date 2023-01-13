using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Application.Queries.ViewModels;

namespace NerdStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerMyBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IPedidoQueries _pedidoQueries;
        private readonly IMediaTrHandler _mediatorHandler;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
                                  IProdutoAppService produtoAppService,
                                  IMediaTrHandler mediatorHandler,
                                  IPedidoQueries pedidoQueries) : base(notifications, mediatorHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatorHandler = mediatorHandler;
            _pedidoQueries = pedidoQueries;
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest();

            if (produto.QuantidadeEstoque < quantidade)
            {
                return BadRequest("Produto com estoque insuficiente");
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return Ok();
            }

            return BadRequest(ObterMensagensErro());
        }

        [HttpGet("meu-carrinho")]
        public async Task<IActionResult> GetCarrinho()
        {
            return Ok(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpDelete("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest("Produto nao encontrado");

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok();

            return BadRequest("Erro ao remover item");
        }

        [HttpPut("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest();

            var command = new AtualizarItemPedidoCommand(ClienteId, id, quantidade);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok();

            return BadRequest("Erro ao atualizar item");
        }

        [HttpPost("aplicar-voucher")]
        public async Task<IActionResult> AtualizarItem(string voucher)
        {
            var command = new AplicarVoucherPedidoCommand(ClienteId, voucher);
            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
                return Ok();

            return BadRequest("Erro ao atualizar item");
        }

        [HttpPost("iniciar-pedido")]
        public async Task<IActionResult> IniciarPedido(CarrinhoViewModel carrinhoViewModel)
        {
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);

            var command = new IniciarPedidoCommand(carrinho.PedidoId, ClienteId, carrinho.ValorTotal, carrinhoViewModel.Pagamento.NomeCartao,
                carrinhoViewModel.Pagamento.NumeroCartao, carrinhoViewModel.Pagamento.ExpiracaoCartao, carrinhoViewModel.Pagamento.CvvCartao);

            await _mediatorHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}
