using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Filters;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar Azure Key Vault a la configuración
var keyVaultEndpoint = builder.Configuration["KeyVault:Endpoint"];
if (string.IsNullOrEmpty(keyVaultEndpoint))
{
    throw new InvalidOperationException("The Key Vault endpoint is not configured.");
}
builder.Configuration.AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential());

// Resto de la configuración
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://proud-pebble-0419a7e0f.5.azurestaticapps.net") // Servidor de desarrollo de React
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Registro de ApiKeyService
builder.Services.AddSingleton<ApiKeyService>();

// Configuración de autenticación JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // Minimizar el desfase del reloj para prevenir ataques de repetición
    };
});

// Registro del filtro de autenticación de la API Key
builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddAzureWebAppDiagnostics();
});
var app = builder.Build();

// Configurar el pipeline de la solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(); // Habilitar CORS
app.UseMiddleware<WebApplication1.Middleware.RateLimitingMiddleware>(); // Registrar middleware de limitación de tasa
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
