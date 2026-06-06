// ==========================================================================
// Controller: AuthController.cs
// Responsável pela autenticação (login) dos usuários.
// Aqui temos o endpoint que recebe e-mail/senha e devolve um token JWT.
//
// [ApiController] = diz ao ASP.NET que esta é uma classe de API.
// [Route("api/[controller]")] = define a URL base: /api/auth
// ==========================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockLite.Api.Data;
using StockLite.Api.DTOs;
using StockLite.Api.Services;

namespace StockLite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Dependências injetadas pelo ASP.NET (Injeção de Dependência)
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    // -----------------------------------------------------------------------
    // O ASP.NET injeta automaticamente o banco (AppDbContext) e o serviço
    // de tokens (ITokenService). Note que usamos a INTERFACE, não a classe
    // concreta — isso é o princípio "D" do SOLID em ação!
    // -----------------------------------------------------------------------
    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    // -----------------------------------------------------------------------
    // POST /api/auth/login
    // Endpoint de login: recebe e-mail e senha, valida e retorna um token JWT.
    //
    // [HttpPost("login")] = este método responde a requisições POST na rota
    // /api/auth/login
    //
    // [FromBody] = o ASP.NET lê o corpo (body) da requisição JSON e preenche
    // o objeto LoginRequest automaticamente.
    // -----------------------------------------------------------------------
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // PASSO 1: Buscar o usuário no banco pelo e-mail E senha.
        // ★ LINQ EM AÇÃO ★ — FirstOrDefaultAsync é um método LINQ!
        // Ele busca o PRIMEIRO registro que atende à condição, ou null.
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                u.Senha == request.Senha
            );

        // PASSO 2: Se não encontrou, retorna 401 (Não Autorizado)
        if (usuario == null)
        {
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
        }

        // PASSO 3: Gerar o token JWT usando o serviço
        var token = _tokenService.GerarToken(usuario);

        // PASSO 4: Retornar 200 (OK) com o token
        var resposta = new LoginResponse
        {
            Token = token,
            Mensagem = "Login realizado com sucesso!"
        };

        return Ok(resposta);
    }
}
