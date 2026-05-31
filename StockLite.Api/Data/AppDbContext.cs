// ==========================================================================
// Data: AppDbContext.cs
// Esta classe é o "coração" do Entity Framework Core.
// Ela herda de DbContext e representa a conexão com o banco de dados.
// Cada propriedade DbSet<T> vira uma "tabela" no banco.
//
// CONCEITO: Code-First — escrevemos as classes C# primeiro, e o EF Core
// cria/atualiza o banco automaticamente com base nelas.
// ==========================================================================

using Microsoft.EntityFrameworkCore;
using StockLite.Api.Models;

namespace StockLite.Api.Data;

public class AppDbContext : DbContext
{
    // -----------------------------------------------------------------------
    // O construtor recebe as opções de configuração (ex: qual banco usar).
    // Essa injeção é feita automaticamente pelo ASP.NET (Injeção de Dependência).
    // -----------------------------------------------------------------------
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // -----------------------------------------------------------------------
    // DbSet = representa uma tabela no banco de dados.
    // "Usuarios" será a tabela de usuários.
    // "Produtos" será a tabela de produtos.
    // -----------------------------------------------------------------------
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Produto> Produtos { get; set; } = null!;

    // -----------------------------------------------------------------------
    // OnModelCreating: Método chamado quando o EF cria o modelo do banco.
    // Aqui podemos configurar regras extras (ex: dados iniciais/seed).
    // -----------------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ----- SEED DATA (Dados Iniciais) -----
        // Inserimos um usuário padrão para facilitar o teste de login.
        // Em produção, isso viria de um processo de cadastro real.
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Nome = "Admin",
                Email = "admin@stocklite.com",
                Senha = "123456" // ATENÇÃO: em produção, use hash de senha!
            }
        );

        // Inserimos alguns produtos de exemplo para testar as consultas
        modelBuilder.Entity<Produto>().HasData(
            new Produto
            {
                Id = 1,
                Nome = "Teclado Mecânico",
                Descricao = "Teclado gamer com switches blue",
                Preco = 299.90m,
                QuantidadeEstoque = 50
            },
            new Produto
            {
                Id = 2,
                Nome = "Mouse Wireless",
                Descricao = "Mouse sem fio com sensor óptico",
                Preco = 89.90m,
                QuantidadeEstoque = 5 // Estoque baixo! (< 10)
            },
            new Produto
            {
                Id = 3,
                Nome = "Monitor 24 polegadas",
                Descricao = "Monitor Full HD IPS",
                Preco = 899.00m,
                QuantidadeEstoque = 3 // Estoque baixo! (< 10)
            },
            new Produto
            {
                Id = 4,
                Nome = "Webcam HD",
                Descricao = "Webcam 1080p com microfone",
                Preco = 159.90m,
                QuantidadeEstoque = 25
            }
        );
    }
}
