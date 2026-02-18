using BLL_Circulares;
using DAL_Circulares;
using Entities_Circulares.EmailSettings;
using Utils_Circulares.Correos;
using Utils_Circulares.GeneradorAleatorio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Abierto", p => p
        .AllowAnyOrigin()   // <-- clave: permite cualquier dominio
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddScoped<InicioBLL>();
builder.Services.AddScoped<InicioDAL>();
builder.Services.AddScoped<GestionUsuariosBLL>();
builder.Services.AddScoped<GestionUsuariosDAL>();
builder.Services.AddScoped<PublicarCircularBLL>();
builder.Services.AddScoped<PublicarCircularDAL>();
builder.Services.AddScoped<CircularesBLL>();
builder.Services.AddScoped<CircularesDAL>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<EmailService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseCors("Abierto");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/HomeAPI/API_Circulares"));

// ⬅️ Ruta convencional para Vistas (Home/Index por defecto)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomeAPI}/{action=API_Circulares}/{id?}");

// ⬅️ Tus controladores con rutas por atributo siguen funcionando
app.MapControllers();

app.Run();
