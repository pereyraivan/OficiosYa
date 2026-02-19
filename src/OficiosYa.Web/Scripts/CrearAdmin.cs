using BCrypt.Net;
using OficiosYa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace OficiosYa.Web.Scripts
{
    public class CrearAdmin
    {
        public static async Task Ejecutar(string[] args)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=OficiosYa_Database;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            
            using var context = new ApplicationDbContext(optionsBuilder.Options);
            
            // Contraseña del admin
            const string password = "Admin123!";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            
            Console.WriteLine($"Hash generado: {passwordHash}");
            Console.WriteLine();
            
            // Verificar si el usuario existe
            var admin = await context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == "admin@oficiosya.com");
            
            if (admin != null)
            {
                admin.PasswordHash = passwordHash;
                admin.EsAdmin = true;
                admin.Activo = true;
                admin.EmailVerificado = true;
                admin.Nombre = "Administrador";
                admin.Apellido = "Sistema";
                
                context.Usuarios.Update(admin);
                Console.WriteLine("Usuario administrador actualizado.");
            }
            else
            {
                var nuevoAdmin = new Domain.Entities.Usuario
                {
                    Nombre = "Administrador",
                    Apellido = "Sistema",
                    Email = "admin@oficiosya.com",
                    PasswordHash = passwordHash,
                    EsAdmin = true,
                    Activo = true,
                    EmailVerificado = true,
                    FechaRegistro = DateTime.UtcNow
                };
                
                context.Usuarios.Add(nuevoAdmin);
                Console.WriteLine("Usuario administrador creado.");
            }
            
            await context.SaveChangesAsync();
            
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("CREDENCIALES DE ACCESO:");
            Console.WriteLine("Email: admin@oficiosya.com");
            Console.WriteLine("Contraseña: Admin123!");
            Console.WriteLine("========================================");
        }
    }
}

