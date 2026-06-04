// ==========================================================================
// Service: ProdutoService.cs
// Implementação concreta da interface IProdutoService.
// Contém toda a lógica de negócio relacionada a Produtos.
//
// PRINCÍPIO SOLID — "S" de Single Responsibility:
// Esta classe cuida SOMENTE da lógica de produtos.
// Ela não sabe nada sobre autenticação, tokens, etc.
//
// PRINCÍPIO SOLID — "D" de Dependency Inversion:
// Dependemos da abstração (IProdutoService), não da implementação.
// ==========================================================================

using Microsoft.EntityFrameworkCore;
using StockLite.Api.Data;
using StockLite.Api.Models;

namespace StockLite.Api.Services;

public class ProdutoService : IProdutoService
{
    // Referência ao banco de dados (injetada pelo ASP.NET)
    private readonly AppDbContext _context;

    // -----------------------------------------------------------------------
    // Injeção de Dependência: o ASP.NET cria o AppDbContext e injeta aqui.
    // Não precisamos fazer "new AppDbContext()" manualmente.
    // -----------------------------------------------------------------------
    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    // -----------------------------------------------------------------------
    // Retorna TODOS os produtos do banco de dados.
    // ToListAsync() = executa a query de forma assíncrona (não trava o servidor).
    // -----------------------------------------------------------------------
    public async Task<List<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos.ToListAsync();
    }

    // -----------------------------------------------------------------------
    // Busca um produto pelo Id.
    // FindAsync() = busca pela chave primária de forma otimizada.
    // Retorna null se não encontrar.
    // -----------------------------------------------------------------------
    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produtos.FindAsync(id);
    }

    // -----------------------------------------------------------------------
    // ★ LINQ EM AÇÃO! ★
    // Busca produtos com estoque menor que 10 unidades.
    // Where() = filtra os dados (equivalente ao WHERE do SQL).
    // Isso demonstra o uso de LINQ com Entity Framework.
    // -----------------------------------------------------------------------
    public async Task<List<Produto>> ObterEstoqueBaixoAsync()
    {
        // LINQ: "Pegue todos os produtos ONDE a quantidade em estoque for < 10"
        return await _context.Produtos
            .Where(p => p.QuantidadeEstoque < 10)  // Filtro LINQ
            .OrderBy(p => p.QuantidadeEstoque)      // Ordena do menor para o maior
            .ToListAsync();
    }

    // -----------------------------------------------------------------------
    // Cadastra um novo produto no banco.
    // Primeiro valida usando a regra de negócio da Model (EhValido).
    // -----------------------------------------------------------------------
    public async Task<Produto?> CadastrarAsync(Produto produto)
    {
        // Validação usando a regra de negócio do Model
        if (!produto.EhValido())
        {
            // Retorna null para indicar que o produto é inválido
            return null;
        }

        // Adiciona o produto ao contexto do EF (ainda não salvou no banco)
        _context.Produtos.Add(produto);

        // SaveChangesAsync() = persiste as mudanças no banco de dados
        await _context.SaveChangesAsync();

        // Retorna o produto com o Id gerado pelo banco
        return produto;
    }

    // -----------------------------------------------------------------------
    // Atualiza um produto existente.
    // -----------------------------------------------------------------------
    public async Task<bool> AtualizarAsync(Produto produto)
    {
        // Verifica se o produto existe no banco
        var produtoExistente = await _context.Produtos.FindAsync(produto.Id);
        if (produtoExistente == null)
        {
            return false; // Produto não encontrado
        }

        // Atualiza os campos
        produtoExistente.Nome = produto.Nome;
        produtoExistente.Descricao = produto.Descricao;
        produtoExistente.Preco = produto.Preco;
        produtoExistente.QuantidadeEstoque = produto.QuantidadeEstoque;

        await _context.SaveChangesAsync();
        return true; // Atualizado com sucesso
    }

    // -----------------------------------------------------------------------
    // Remove um produto pelo Id.
    // -----------------------------------------------------------------------
    public async Task<bool> RemoverAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return false; // Produto não encontrado
        }

        // Remove do contexto
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return true; // Removido com sucesso
    }
}
