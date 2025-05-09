using System.Text;
using ApiVault.Data;
using ApiVault.Interfaces;
using ApiVault.Services;
using ApiVault.Settings;
using ApiVault.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings")); // Configurar las opciones de ApiSettings

var claveSecreta = builder.Configuration.GetSection("ApiSettings:Secreta").Value;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
{
    op.RequireHttpsMetadata = false;
    op.SaveToken = true;
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["ApiSettings:Emisor"],
        ValidAudience = builder.Configuration["ApiSettings:Audiencia"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta)),

    };
});

var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");

// Agregar servicios al contenedor
// Configurar el contexto de base de datos con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));
//Inyectando helper JWT
builder.Services.AddScoped<JwtHelper>();


// Inyectar el servicio de productos
builder.Services.AddScoped<IProductoService, ProductoService>();
// Inyectar el servicio de carrito
builder.Services.AddScoped<ICarritoService, CarritoService>();
// Inyectar el servicio de pedidos
builder.Services.AddScoped<IPedidoService, PedidoService>();
// Inyectar el servicio de tipos de productos y marcas 
builder.Services.AddScoped<ITipoProductoService, TipoProductoService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
// Inyectar el servicio de usuarios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();




// Agregar otros servicios que necesites
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "JWT Authorization header usando el esquema 'Bearer'. \n\r\n\r" +
        "Ingrese 'Bearer' [espacio] y luego su token en el campo de texto a continuación.\n\r\n\r" +
        "Ejemplo: \"Bearer tutoken\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddScoped<IAuthService, AuthService>(); // Agregar el servicio de autenticación

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
