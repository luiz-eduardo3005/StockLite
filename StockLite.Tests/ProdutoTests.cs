// ==========================================================================
// Testes Unitários: ProdutoTests.cs
// Usamos xUnit para testar as regras de negócio da classe Produto.
//
// O QUE É TESTE UNITÁRIO?
// É um teste que verifica UMA unidade de código isolada.
// Aqui testamos os métodos EhValido() e EstoqueBaixo() do Model Produto.
// Não precisamos de banco de dados, API, ou internet — é 100% isolado.
//
// PADRÃO AAA (Arrange, Act, Assert):
// - Arrange (Preparar): cria os objetos necessários
// - Act (Agir): executa o método que queremos testar
// - Assert (Verificar): confere se o resultado é o esperado
// ==========================================================================

using StockLite.Api.Models;

namespace StockLite.Tests;

public class ProdutoTests
{
    // -----------------------------------------------------------------------
    // TESTE 1: Produto com preço negativo deve ser inválido.
    // Um produto que custa -50 reais não faz sentido!
    // -----------------------------------------------------------------------
    [Fact]
    public void EhValido_PrecoNegativo_DeveRetornarFalse()
    {
        // Arrange (Preparar): criamos um produto com preço negativo
        var produto = new Produto
        {
            Nome = "Produto Teste",
            Preco = -50.00m,  // Preço NEGATIVO (inválido!)
            QuantidadeEstoque = 10
        };

        // Act (Agir): chamamos o método que queremos testar
        var resultado = produto.EhValido();

        // Assert (Verificar): o resultado deve ser FALSE
        Assert.False(resultado);
    }

    // -----------------------------------------------------------------------
    // TESTE 2: Produto com preço zero deve ser válido.
    // Um brinde/amostra grátis pode ter preço zero.
    // -----------------------------------------------------------------------
    [Fact]
    public void EhValido_PrecoZero_DeveRetornarTrue()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Brinde Grátis",
            Preco = 0m,  // Preço ZERO (válido!)
            QuantidadeEstoque = 100
        };

        // Act
        var resultado = produto.EhValido();

        // Assert
        Assert.True(resultado);
    }

    // -----------------------------------------------------------------------
    // TESTE 3: Produto com preço positivo e nome preenchido deve ser válido.
    // Este é o cenário "feliz" — tudo correto.
    // -----------------------------------------------------------------------
    [Fact]
    public void EhValido_PrecoPositivoENomePreenchido_DeveRetornarTrue()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Teclado Mecânico",
            Preco = 299.90m,  // Preço POSITIVO (válido!)
            QuantidadeEstoque = 50
        };

        // Act
        var resultado = produto.EhValido();

        // Assert
        Assert.True(resultado);
    }

    // -----------------------------------------------------------------------
    // TESTE 4: Produto sem nome deve ser inválido.
    // Um produto precisa ter nome para ser identificado.
    // -----------------------------------------------------------------------
    [Fact]
    public void EhValido_NomeVazio_DeveRetornarFalse()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "",        // Nome VAZIO (inválido!)
            Preco = 100.00m,
            QuantidadeEstoque = 10
        };

        // Act
        var resultado = produto.EhValido();

        // Assert
        Assert.False(resultado);
    }

    // -----------------------------------------------------------------------
    // TESTE 5: Produto com estoque menor que 10 deve ter estoque baixo.
    // -----------------------------------------------------------------------
    [Fact]
    public void EstoqueBaixo_MenosDe10Unidades_DeveRetornarTrue()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Mouse",
            Preco = 50.00m,
            QuantidadeEstoque = 5  // Menos de 10! (estoque baixo)
        };

        // Act
        var resultado = produto.EstoqueBaixo();

        // Assert
        Assert.True(resultado);
    }

    // -----------------------------------------------------------------------
    // TESTE 6: Produto com estoque igual ou maior que 10 NÃO tem estoque baixo.
    // -----------------------------------------------------------------------
    [Fact]
    public void EstoqueBaixo_MaisQue10Unidades_DeveRetornarFalse()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Teclado",
            Preco = 150.00m,
            QuantidadeEstoque = 25  // Mais de 10 (estoque OK)
        };

        // Act
        var resultado = produto.EstoqueBaixo();

        // Assert
        Assert.False(resultado);
    }

    // -----------------------------------------------------------------------
    // TESTE 7: Produto com estoque exatamente 10 NÃO deve ter estoque baixo.
    // "Menor que 10" exclui o 10.
    // -----------------------------------------------------------------------
    [Fact]
    public void EstoqueBaixo_Exatamente10Unidades_DeveRetornarFalse()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Monitor",
            Preco = 800.00m,
            QuantidadeEstoque = 10  // Exatamente 10 (NÃO é baixo)
        };

        // Act
        var resultado = produto.EstoqueBaixo();

        // Assert
        Assert.False(resultado);
    }
}
