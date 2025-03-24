using PedidoApi.DataAccess;
using PedidoApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClienteDAO, ClienteDAO>();
builder.Services.AddScoped<IProdutoDAO, ProdutoDAO>();
builder.Services.AddScoped<IPedidoDAO, PedidoDAO>();
builder.Services.AddScoped<IAuthDAO, AuthDAO>();


// Configuração do JWT
var key = Encoding.ASCII.GetBytes("cApG5jY6c2EJ");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "aI6b57jMxktD",
        ValidAudience = "TdL4Wo17ErBR",
        IssuerSigningKey = new SymmetricSecurityKey(key),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
        {
            var jwtToken = securityToken as JwtSecurityToken;
            using (var scope = builder.Services.BuildServiceProvider().CreateScope())
            {
                var authDAO = scope.ServiceProvider.GetRequiredService<IAuthDAO>();
                var revogado = authDAO.TokenRenogado(jwtToken.RawData);

                return expires != null && expires > DateTime.UtcNow && !revogado;
            }
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
