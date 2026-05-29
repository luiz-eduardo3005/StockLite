// ==========================================================================
// Model: Usuario.cs
// Representa a entidade "Usuário" no banco de dados.
// Aqui usamos POO com encapsulamento: as propriedades são públicas
// para que o Entity Framework consiga mapear, mas seguimos o princípio
// de manter a classe simples e com responsabilidade única (SRP do SOLID).
// ==========================================================================

namespace StockLite.Api.Models;

public class Usuario
{
    // Chave primária — o EF Core reconhece "Id" automaticamente como PK
    public int Id { get; set; }

    // Nome do usuário (ex: "Eduardo")
    public string Nome { get; set; } = string.Empty;

    // E-mail usado para login
    public string Email { get; set; } = string.Empty;

    // Senha armazenada (em produção, NUNCA guarde senha em texto puro!
    // Aqui simplificamos para fins didáticos)
    public string Senha { get; set; } = string.Empty;
}
