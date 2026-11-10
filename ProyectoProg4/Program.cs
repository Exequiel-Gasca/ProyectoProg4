using Application;
using Infraestructure;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.Services;


var builder = WebApplication.CreateBuilder(args);
// Agregar el ApplicationDbContext al contenedor de servicios
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
// (los AddScoped de repositorios y servicios se agregan en el paso 11)
builder.Services.AddHttpClient(HttpClientNames.MercadoPago, client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:MercadoPago:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(10);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    string? accessToken = builder.Configuration["ExternalApis:MercadoPago:AccessToken"];
if (!string.IsNullOrWhiteSpace(accessToken))
    {
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

});

builder.Services.AddScoped<IMercadoPagoService,MercadoPagoServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "API v1"
        );
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();