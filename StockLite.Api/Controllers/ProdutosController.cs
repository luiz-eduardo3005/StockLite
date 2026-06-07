// ==========================================================================
// Controller: ProdutosController.cs
// Responsável por todas as operações com Produtos (CRUD completo).
// Demonstra: endpoints REST, LINQ, JWT (Authorize) e Injeção de Dependência.
//
// Rotas REST padrão:
//   GET    /api/produtos           → Lista todos os produtos
//   GET    /api/produtos/5         → Busca produto com Id 5
//   GET    /api/produtos/estoque-baixo → Produtos com estoque < 10 (LINQ!)
//   POST   /api/produtos           → Cadastra novo produto [Authorize]
//   PUT    /api/produtos/5         → Atualiza produto com Id 5 [Authorize]
//   DELETE /api/produtos/5         → Remove produto com Id 5  [Authorize]
// ==========================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockLite.Api.Models;
using StockLite.Api.Services;

namespace StockLite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    // Dependência injetada — usamos a INTERFACE, não a classe concreta
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    // -----------------------------------------------------------------------
    // GET /api/produtos
    // Retorna todos os produtos cadastrados.
    // Não precisa de autenticação (é público).
    // -----------------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var produtos = await _produtoService.ObterTodosAsync();
        return Ok(produtos);
    }

    // -----------------------------------------------------------------------
    // GET /api/produtos/{id}
    // Retorna um produto específico pelo Id.
    // Se não encontrar, retorna 404 (Not Found).
    // -----------------------------------------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);

        if (produto == null)
        {
            return NotFound(new { mensagem = $"Produto com Id {id} não encontrado." });
        }

        return Ok(produto);
    }

    // -----------------------------------------------------------------------
    // ★ GET /api/produtos/estoque-baixo ★
    // Retorna produtos com estoque menor que 10 unidades.
    // Aqui usamos LINQ no Service para filtrar os dados!
    // -----------------------------------------------------------------------
    [HttpGet("estoque-baixo")]
    public async Task<IActionResult> ObterEstoqueBaixo()
    {
        var produtos = await _produtoService.ObterEstoqueBaixoAsync();
        return Ok(produtos);
    }

    // -----------------------------------------------------------------------
    // ★ POST /api/produtos ★
    // Cadastra um novo produto.
    //
    // [Authorize] = ESTE ENDPOINT É PROTEGIDO POR JWT!
    // O cliente precisa enviar o token no header:
    //   Authorization: Bearer <seu-token-aqui>
    //
    // Se não enviar o token, recebe 401 (Unauthorized).
    // -----------------------------------------------------------------------
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Cadastrar([FromBody] Produto produto)
    {
        var novoProduto = await _produtoService.CadastrarAsync(produto);

        if (novoProduto == null)
        {
            // A regra de negócio (EhValido) impediu o cadastro
            return BadRequest(new { mensagem = "Produto inválido. Verifique o nome e o preço (não pode ser negativo)." });
        }

        // Retorna 201 (Created) com a URL do novo recurso
        // CreatedAtAction = retorna o header "Location" apontando para GET /api/produtos/{id}
        return CreatedAtAction(
            nameof(ObterPorId),        // Nome do método GET
            new { id = novoProduto.Id }, // Parâmetro da rota
            novoProduto                 // Corpo da resposta
        );
    }

    // -----------------------------------------------------------------------
    // PUT /api/produtos/{id}
    // Atualiza um produto existente. Também protegido por JWT.
    // -----------------------------------------------------------------------
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Produto produto)
    {
        // Garante que o Id da URL é o mesmo do corpo
        produto.Id = id;

        var atualizado = await _produtoService.AtualizarAsync(produto);

        if (!atualizado)
        {
            return NotFound(new { mensagem = $"Produto com Id {id} não encontrado." });
        }

        return NoContent(); // 204 = atualizado com sucesso, sem corpo na resposta
    }

    // -----------------------------------------------------------------------
    // DELETE /api/produtos/{id}
    // Remove um produto. Protegido por JWT.
    // -----------------------------------------------------------------------
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var removido = await _produtoService.RemoverAsync(id);

        if (!removido)
        {
            return NotFound(new { mensagem = $"Produto com Id {id} não encontrado." });
        }

        return NoContent(); // 204 = removido com sucesso
    }
}
