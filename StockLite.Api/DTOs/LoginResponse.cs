// ==========================================================================
// DTO: LoginResponse.cs
// Objeto que será devolvido ao usuário após um login bem-sucedido.
// Contém o token JWT que o front-end usará nas próximas requisições.
// ==========================================================================

namespace StockLite.Api.DTOs;

public class LoginResponse
{
    // O token JWT gerado pelo servidor
    public string Token { get; set; } = string.Empty;

    // Mensagem amigável (ex: "Login realizado com sucesso!")
    public string Mensagem { get; set; } = string.Empty;
}
