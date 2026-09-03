using Microsoft.EntityFrameworkCore;
using OficiosYa.Persistence.Context;
using OficiosYa.Persistence.Repositories;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Application.Mappings;
using MediatR;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OficiosYa.Web.Services;
using OficiosYa.Web.Options;

var builder = WebApplication.CreateBuilder(args);
// Solo en desarrollo: overrides locales (SMTP, FrontendBaseUrl localhost, etc.).
// Nunca cargar Local.json en producción: si se sube a Somee pisa Somee con localhost.
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
}

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

// Registrar Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IProfesionalRepository, ProfesionalRepository>();
builder.Services.AddScoped<ICategoriaServicioRepository, CategoriaServicioRepository>();
builder.Services.AddScoped<ITipoServicioRepository, TipoServicioRepository>();
builder.Services.AddScoped<IPlanSuscripcionRepository, PlanSuscripcionRepository>();

// Registrar Services
builder.Services.AddScoped<OficiosYa.Application.Services.IPlanSuscripcionService, OficiosYa.Application.Services.PlanSuscripcionService>();
builder.Services.AddHttpClient<IOpenAIModerationService, OpenAIModerationService>();
builder.Services.AddHttpClient<IImageModerationService, SightengineImageModerationService>();
builder.Services.Configure<TransferenciaSuscripcionOptions>(builder.Configuration.GetSection("TransferenciaSuscripcion"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("App"));
builder.Services.Configure<OficiosYa.Application.Options.LanzamientoPromoOptions>(
    builder.Configuration.GetSection(OficiosYa.Application.Options.LanzamientoPromoOptions.SectionName));
builder.Services.AddScoped<OficiosYa.Application.Services.IPasswordResetEmailService, PasswordResetEmailService>();

// JWT
builder.Services.AddScoped<JwtTokenService>();
var jwtSecret = builder.Configuration["Jwt:SecretKey"] ?? "OficiosYa_SecretKey_Minimo32Caracteres_ParaProduccionCambiar";
var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtKey,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "OficiosYa",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "OficiosYa",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Configurar Swagger/OpenAPI (solo en desarrollo)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT: Bearer {token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

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
            var corsOrigins = builder.Configuration.GetSection("App:CorsOrigins").Get<string[]>()
                ?? new[]
                {
                    "http://localhost:5173",
                    "https://tuoficiopro.com.ar",
                    "http://tuoficiopro.com.ar",
                    "https://www.tuoficiopro.com.ar",
                    "http://www.tuoficiopro.com.ar",
                    "http://oficiosya.somee.com",
                    "https://oficiosya.somee.com"
                };

            policy.WithOrigins(corsOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

var app = builder.Build();

var emailEnabled = builder.Configuration.GetValue<bool>("Email:Enabled");
var emailUser = builder.Configuration["Email:Username"] ?? "(vacío)";
app.Logger.LogInformation(
    "Email SMTP al iniciar: Enabled={Enabled}, Username={Username}, Host={Host}",
    emailEnabled,
    emailUser,
    builder.Configuration["Email:SmtpHost"]);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// En Somee free suele usarse HTTP; no forzar HTTPS (rompe el sitio).
if (!app.Environment.IsDevelopment()
    && builder.Configuration.GetValue("App:ForceHttps", false))
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TuOficioPro API v1"));
}

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Mapear controladores API
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Vue SPA (history mode): cualquier ruta no API/MVC → wwwroot/index.html
app.MapFallbackToFile("index.html");
// Compatibilidad temporal con builds viejos en /dist/
app.MapFallbackToFile("/dist/{*path}", "dist/index.html");

app.Run();
