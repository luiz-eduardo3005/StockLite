// ==========================================================================
// Service Interface: ITokenService.cs
// 
// PRINCÍPIO SOLID — "D" de Dependency Inversion (Inversão de Dependência):
// O controller não depende da implementação concreta (TokenService),
// mas sim desta INTERFACE (ITokenService).
// Isso permite trocar a implementação no futuro sem alterar o controller.
//
// PRINCÍPIO SOLID — "I" de Interface Segregation:
// A interface é pequena e específica: só gera tokens. Nada mais.
// ==========================================================================

using StockLite.Api.Models;

namespace StockLite.Api.Services;

public interface ITokenService
{
    // Recebe um usuário e retorna um token JWT como string
    string GerarToken(Usuario usuario);
}
