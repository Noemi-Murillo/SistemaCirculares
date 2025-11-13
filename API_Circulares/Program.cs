using BLL_Circulares;
using DAL_Circulares;
using Utils_Circulares.GeneradorAleatorio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<InicioBLL>();
builder.Services.AddScoped<InicioDAL>();
builder.Services.AddScoped<GestionUsuariosBLL>();
builder.Services.AddScoped<GestionUsuariosDAL>();
builder.Services.AddScoped<PublicarCircularBLL>();
builder.Services.AddScoped<PublicarCircularDAL>();
builder.Services.AddScoped<CircularesBLL>();
builder.Services.AddScoped<CircularesDAL>();

var app = builder.Build();

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();

// ⬅️ Ruta convencional para Vistas (Home/Index por defecto)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomeAPI}/{action=API_Circulares}/{id?}");

// ⬅️ Tus controladores con rutas por atributo siguen funcionando
app.MapControllers();

app.Run();
