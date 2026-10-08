using Infraestructure;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
// Agregar el ApplicationDbContext al contenedor de servicios
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
// (los AddScoped de repositorios y servicios se agregan en el paso 11)
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