
using Application;
using Domain.Entities;
using Domain.Enums;
using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using Domain.Interfaces;
using Infraestructure;
using Infraestructure.Repositories;
using Infrastructure.ApisServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ProyectoProg4.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// BASE DE DATOS

bool useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (useInMemory)
        options.UseInMemoryDatabase("Practica1Db");
    else
        options.UseSqlServer(connectionString
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión DefaultConnection."));
});

// REPOSITORIOS

// Categorías
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Clientes
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Pagos
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Productos
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Ventas
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// Detalles de venta
builder.Services.AddScoped<ISaleDetailRepository, SaleDetailRepository>();

// Movimientos de stock
builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();

// Usuarios
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Roles
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();


// SERVICIOS

// Categorías
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Clientes
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Pagos
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Productos
builder.Services.AddScoped<IProductService, ProductService>();

// Ventas
builder.Services.AddScoped<ISaleService, SaleService>();

// Detalles de venta
builder.Services.AddScoped<ISaleDetailService, SaleDetailService>();

// Usuarios
builder.Services.AddScoped<IUserService, UserService>();

// Roles
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

// Sotck Movement

builder.Services.AddScoped<IStockMovementService, StockMovementServices>();

// CONFIGURACION DE AUTORIZACION

builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Falta la configuración Jwt:Key.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
          
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            
            NameClaimType = "sub",
            RoleClaimType = "role",
            ClockSkew = TimeSpan.Zero
        };
    });

// MERCADO PAGO

builder.Services.AddHttpClient(HttpClientNames.MercadoPago, client =>
{
    var baseUrl = builder.Configuration[
        "ExternalApis:MercadoPago:BaseUrl"];

    if (!string.IsNullOrWhiteSpace(baseUrl))
    {
        client.BaseAddress = new Uri(baseUrl);
    }

    client.Timeout = TimeSpan.FromSeconds(10);

    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    string? accessToken = builder.Configuration[
        "ExternalApis:MercadoPago:AccessToken"];

    if (!string.IsNullOrWhiteSpace(accessToken))
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
    }
});

builder.Services.AddScoped<IMercadoPagoService, MercadoPagoServices>();


// CONTROLADORES Y SWAGGER

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegá solo el token, sin la palabra Bearer."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

// CONFIGURACION DE LA APLICACION

var app = builder.Build();

if (useInMemory)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Crea las tablas y carga los roles semilla (Common, Admin, SuperAdmin)
    db.Database.EnsureCreated();

    // Sin base real no hay migraciones: dejamos un SuperAdmin para poder entrar
    if (!db.User.Any())
    {
        db.User.Add(new User
        {
            FirstName = "Super",
            LastName = "Admin",
            Email = builder.Configuration["SeedAdmin:Email"] ?? "admin@proyecto.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                builder.Configuration["SeedAdmin:Password"] ?? "Admin12345!"),
            IsActive = true,
            RoleId = (int)RoleType.SuperAdmin
        });
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
