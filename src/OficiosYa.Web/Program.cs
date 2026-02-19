using Microsoft.EntityFrameworkCore;
using OficiosYa.Persistence.Context;
using OficiosYa.Persistence.Repositories;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Application.Mappings;
using MediatR;
using System.Reflection;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configurar encoder para permitir todos los caracteres Unicode (incluyendo ñ y acentos)
var jsonEncoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = jsonEncoder;
        options.JsonSerializerOptions.WriteIndented = false;
    });

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = jsonEncoder;
        options.JsonSerializerOptions.WriteIndented = false;
    });

// Configurar Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProfesionalRepository, ProfesionalRepository>();
builder.Services.AddScoped<IOficioRepository, OficioRepository>();
builder.Services.AddScoped<ICategoriaServicioRepository, CategoriaServicioRepository>();
builder.Services.AddScoped<ITipoServicioRepository, TipoServicioRepository>();
builder.Services.AddScoped<IPlanSuscripcionRepository, PlanSuscripcionRepository>();

// Registrar Services
builder.Services.AddScoped<OficiosYa.Application.Services.IPlanSuscripcionService, OficiosYa.Application.Services.PlanSuscripcionService>();

// Configurar MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OficiosYa.Application.UseCases.Usuarios.RegistrarUsuario.RegistrarUsuarioHandler).Assembly));

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // En desarrollo permitir cualquier origen para evitar problemas de puerto dinámico de Vite
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5174", "http://localhost:5175")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Solo redirigir HTTPS en producción
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

// Middleware para asegurar charset UTF-8 en respuestas JSON (antes de escribir la respuesta)
app.Use(async (context, next) =>
{
    // Establecer el Content-Type antes de procesar la respuesta
    context.Response.OnStarting(() =>
    {
        if (context.Response.ContentType?.Contains("application/json") == true)
        {
            if (!context.Response.ContentType.Contains("charset"))
            {
                context.Response.ContentType = context.Response.ContentType.Replace("application/json", "application/json; charset=utf-8");
            }
        }
        return Task.CompletedTask;
    });
    
    await next();
});

// Usar CORS - debe estar después de UseRouting y antes de UseAuthorization
app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapStaticAssets();

// Mapear controladores API
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
