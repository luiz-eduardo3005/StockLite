// ==========================================================================
// Service: TokenService.cs
// Implementação concreta da interface ITokenService.
// Responsável por GERAR tokens JWT (JSON Web Token).
//
// O que é JWT? É um "bilhete digital" que o servidor entrega ao cliente
// após o login. O cliente envia esse bilhete em cada requisição futura
// para provar que está autenticado. O servidor valida o bilhete sem
// precisar consultar o banco de dados novamente.
//
// PRINCÍPIO SOLID — "S" de Single Responsibility:
// Esta classe tem UMA única responsabilidade: gerar tokens.
// ==========================================================================

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StockLite.Api.Models;

namespace StockLite.Api.Services;

public class TokenService : ITokenService
{
    // IConfiguration permite ler valores do appsettings.json
    private readonly IConfiguration _configuration;

    // -----------------------------------------------------------------------
    // Construtor com Injeção de Dependência:
    // O ASP.NET injeta automaticamente o IConfiguration aqui.
    // Não precisamos criar o objeto manualmente (new Configuration()).
    // -----------------------------------------------------------------------
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Usuario usuario)
    {
        // ----- PASSO 1: Criar as "Claims" (informações dentro do token) -----
        // Claims são "pedacinhos de informação" sobre o usuário.
        // Quando alguém decodificar o token, verá essas informações.
        var claims = new[]
        {
            // "sub" (subject) = identificador único do usuário
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),

            // "email" = e-mail do usuário
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),

            // "name" = nome do usuário
            new Claim("name", usuario.Nome),

            // "jti" = ID único do token (evita reutilização)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // ----- PASSO 2: Criar a chave de segurança -----
        // Lemos a chave secreta do appsettings.json.
        // Essa chave é usada para "assinar" o token (como um carimbo).
        var chaveSecreta = _configuration["Jwt:ChaveSecreta"]!;
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));

        // ----- PASSO 3: Definir o algoritmo de assinatura -----
        // HmacSha256 é um algoritmo seguro e muito usado.
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        // ----- PASSO 4: Montar o token -----
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Emissor"],      // Quem emitiu o token
            audience: _configuration["Jwt:Audiencia"],    // Para quem o token é válido
            claims: claims,                                // Informações do usuário
            expires: DateTime.UtcNow.AddHours(2),         // Expira em 2 horas
            signingCredentials: credenciais                // Assinatura digital
        );

        // ----- PASSO 5: Converter o token para string e retornar -----
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
