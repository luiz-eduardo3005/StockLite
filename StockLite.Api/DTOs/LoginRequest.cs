// ==========================================================================
// DTO: LoginRequest.cs
// DTO = Data Transfer Object (Objeto de Transferência de Dados).
// Serve apenas para transportar dados do request HTTP para o controller.
// É uma boa prática NÃO usar a entidade do banco direto no endpoint,
// pois assim separamos a "forma" do dado que chega pela API da entidade
// que vai pro banco. Isso é o princípio de "Separação de Responsabilidades".
// ==========================================================================

namespace StockLite.Api.DTOs;

public class LoginRequest
{
    // E-mail enviado pelo usuário na requisição de login
    public string Email { get; set; } = string.Empty;

    // Senha enviada pelo usuário na requisição de login
    public string Senha { get; set; } = string.Empty;
}
