// ==========================================================================
// Model: Produto.cs
// Representa a entidade "Produto" no banco de dados.
// Contém uma regra de negócio simples: o preço não pode ser negativo.
// Isso demonstra encapsulamento (POO) — controlamos como o valor é setado.
// ==========================================================================

using System.ComponentModel.DataAnnotations;

namespace StockLite.Api.Models;

public class Produto
{
    // Chave primária
    public int Id { get; set; }

    // Nome do produto (ex: "Teclado Mecânico")
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    // Descrição opcional do produto
    public string Descricao { get; set; } = string.Empty;

    // Preço unitário do produto
    // Usamos 'decimal' para valores monetários (mais preciso que 'double')
    public decimal Preco { get; set; }

    // Quantidade em estoque
    public int QuantidadeEstoque { get; set; }

    // -----------------------------------------------------------------------
    // REGRA DE NEGÓCIO: Verifica se o produto é válido para ser cadastrado.
    // Um produto com preço negativo não faz sentido, então retornamos false.
    // Esse método será testado no projeto de testes unitários (xUnit).
    // -----------------------------------------------------------------------
    public bool EhValido()
    {
        // O preço deve ser maior ou igual a zero
        // E o nome não pode ser vazio
        return Preco >= 0 && !string.IsNullOrWhiteSpace(Nome);
    }

    // -----------------------------------------------------------------------
    // REGRA DE NEGÓCIO: Verifica se o estoque está baixo (menos de 10 unidades).
    // Será usada em uma consulta LINQ no controller.
    // -----------------------------------------------------------------------
    public bool EstoqueBaixo()
    {
        return QuantidadeEstoque < 10;
    }
}
