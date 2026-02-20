using BLL_Circulares;
using DAL_Circulares;
using SistemaCirculares.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<InicioModel>();
builder.Services.AddScoped<GestionUsuariosModel>();
builder.Services.AddScoped<CircularesModel>();
builder.Services.AddScoped<ComitesModel>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Iniciosesion}/{id?}")
    .WithStaticAssets();


app.Run();
