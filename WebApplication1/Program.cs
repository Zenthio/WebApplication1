using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Filters;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< Updated upstream
//Register the ApiKeyService
=======
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000") // React development server
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Register ApiKeyService
>>>>>>> Stashed changes
builder.Services.AddSingleton<ApiKeyService>();

//Register the ApiKeyAuthFilter
builder.Services.AddScoped<ApiKeyAuthFilter>();

// Configure JWT authentication
<<<<<<< Updated upstream
var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authnetication");
=======
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        ValidIssuer = "your_issuer",
        ValidAudience = "your_audience",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

=======
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // Minimize clock skew to prevent replay attacks
    };
});

// Register ApiKeyAuthFilter
builder.Services.AddScoped<ApiKeyAuthFilter>();

>>>>>>> Stashed changes
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
<<<<<<< Updated upstream

=======
app.UseCors(); // Enable CORS
app.UseMiddleware<WebApplication1.Middleware.RateLimitingMiddleware>(); // Register custom rate limiting middleware
app.UseAuthentication();
>>>>>>> Stashed changes
app.UseAuthorization();

app.MapControllers();

app.Run();
