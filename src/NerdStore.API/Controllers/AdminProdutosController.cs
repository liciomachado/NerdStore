using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;

namespace NerdStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProdutosController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;

        public AdminProdutosController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet("admin-produtos")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _produtoAppService.ObterTodos());
        }

        [HttpPost("novo-produto")]
        public async Task<IActionResult> NovoProduto(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _produtoAppService.AdicionarProduto(produtoViewModel);
            return Ok();
        }

        [HttpPost("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
        {
            if (quantidade > 0)
            {
                await _produtoAppService.ReporEstoque(id, quantidade);
            }
            else
            {
                await _produtoAppService.DebitarEstoque(id, quantidade);
            }

            return Ok(await _produtoAppService.ObterTodos());
        }

    }
}
