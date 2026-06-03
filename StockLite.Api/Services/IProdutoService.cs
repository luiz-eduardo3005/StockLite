// ==========================================================================
// Service Interface: IProdutoService.cs
// Interface que define as operações disponíveis para Produtos.
// O Controller vai depender desta interface, não da classe concreta.
// ==========================================================================

using StockLite.Api.Models;

namespace StockLite.Api.Services;

public interface IProdutoService
{
    // Retorna todos os produtos
    Task<List<Produto>> ObterTodosAsync();

    // Retorna um produto pelo Id (ou null se não existir)
    Task<Produto?> ObterPorIdAsync(int id);

    // Retorna produtos com estoque baixo (< 10 unidades) — usa LINQ!
    Task<List<Produto>> ObterEstoqueBaixoAsync();

    // Cadastra um novo produto e retorna ele com o Id gerado
    Task<Produto?> CadastrarAsync(Produto produto);

    // Atualiza um produto existente
    Task<bool> AtualizarAsync(Produto produto);

    // Remove um produto pelo Id
    Task<bool> RemoverAsync(int id);
}
