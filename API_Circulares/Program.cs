using BLL_Circulares;
using DAL_Circulares;
using Utils_Circulares.GeneradorAleatorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<InicioBLL>();
builder.Services.AddScoped<InicioDAL>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
