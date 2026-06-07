// ==========================================================================
// Program.cs — PONTO DE ENTRADA DA APLICAÇÃO
// 
// Este arquivo é o "main" do projeto. Aqui configuramos:
// 1. Serviços (Injeção de Dependência)
// 2. Banco de Dados (Entity Framework + SQLite)
// 3. Autenticação JWT
// 4. Pipeline de requisições HTTP
//
// O .NET 8 usa o padrão "Minimal Hosting Model" (sem Startup.cs).
// Tudo é configurado aqui de forma linear e simples.
// ==========================================================================

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockLite.Api.Data;
using StockLite.Api.Services;

// ============================================================
// PASSO 1: Criar o "Builder" da aplicação
// O builder é como um "construtor de prédio" — configuramos
// tudo antes de "construir" (Build) a aplicação.
// ============================================================
var builder = WebApplication.CreateBuilder(args);

// ============================================================
// PASSO 2: Registrar os serviços no container de DI
// (Dependency Injection = Injeção de Dependência)
//
// O que é DI? É um padrão onde NÃO criamos objetos com "new".
// Em vez disso, dizemos ao ASP.NET: "quando alguém pedir um
// ITokenService, crie e entregue um TokenService".
// ============================================================

// Registra os Controllers (classes que respondem às requisições HTTP)
builder.Services.AddControllers();

// ----- ENTITY FRAMEWORK: Configuração do Banco de Dados -----
// UseSqlite() = configura o EF para usar o banco SQLite.
// O arquivo "stocklite.db" será criado automaticamente na raiz do projeto.
// SQLite é perfeito para desenvolvimento: não precisa instalar nada!
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=stocklite.db")
);

// ----- INJEÇÃO DE DEPENDÊNCIA: Registrar nossos serviços -----
// AddScoped = cria UMA instância por requisição HTTP.
// Quando o Controller pedir ITokenService, o ASP.NET entrega um TokenService.
// Quando o Controller pedir IProdutoService, o ASP.NET entrega um ProdutoService.
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

// ----- JWT: Configuração da Autenticação -----
// Lê as configurações do appsettings.json
var chaveSecreta = builder.Configuration["Jwt:ChaveSecreta"]!;
var emissor = builder.Configuration["Jwt:Emissor"]!;
var audiencia = builder.Configuration["Jwt:Audiencia"]!;

builder.Services.AddAuthentication(options =>
{
    // Define que o esquema padrão de autenticação é JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configura como o ASP.NET valida os tokens JWT recebidos
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,              // Valida quem emitiu o token
        ValidateAudience = true,            // Valida para quem o token é
        ValidateLifetime = true,            // Valida se o token não expirou
        ValidateIssuerSigningKey = true,    // Valida a assinatura digital
        ValidIssuer = emissor,              // Emissor esperado
        ValidAudience = audiencia,          // Audiência esperada
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(chaveSecreta)  // Mesma chave usada para gerar
        )
    };
});

// Adiciona o serviço de autorização (necessário para [Authorize] funcionar)
builder.Services.AddAuthorization();

// ============================================================
// PASSO 3: Construir a aplicação
// ============================================================
var app = builder.Build();

// ============================================================
// PASSO 4: Garantir que o banco de dados existe
// EnsureCreated() cria o banco e as tabelas automaticamente
// se eles ainda não existirem (abordagem Code-First).
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// ============================================================
// PASSO 5: Configurar o pipeline de requisições HTTP
// A ORDEM importa! Autenticação vem antes de Autorização.
// ============================================================

// Em ambiente de desenvolvimento, mostra detalhes de erros
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();  // Redireciona HTTP para HTTPS

app.UseAuthentication();    // PRIMEIRO: verifica se o token JWT é válido
app.UseAuthorization();     // SEGUNDO: verifica se o usuário tem permissão

app.MapControllers();       // Mapeia as rotas dos Controllers

// ============================================================
// PASSO 6: Iniciar a aplicação!
// ============================================================
Console.WriteLine("===========================================");
Console.WriteLine("  StockLite API está rodando!");
Console.WriteLine("  URL: http://localhost:5164");
Console.WriteLine("===========================================");
Console.WriteLine();
Console.WriteLine("  Endpoints disponíveis:");
Console.WriteLine("  POST   /api/auth/login           → Login (gera token JWT)");
Console.WriteLine("  GET    /api/produtos              → Listar todos os produtos");
Console.WriteLine("  GET    /api/produtos/{id}         → Buscar produto por Id");
Console.WriteLine("  GET    /api/produtos/estoque-baixo → Produtos com estoque < 10");
Console.WriteLine("  POST   /api/produtos              → Cadastrar produto [JWT]");
Console.WriteLine("  PUT    /api/produtos/{id}         → Atualizar produto [JWT]");
Console.WriteLine("  DELETE /api/produtos/{id}         → Remover produto [JWT]");
Console.WriteLine("===========================================");

app.Run();
