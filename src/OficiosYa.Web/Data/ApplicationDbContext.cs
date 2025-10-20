using Microsoft.EntityFrameworkCore;
using OficiosYa.Web.Models;

namespace OficiosYa.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Oficio> Oficios { get; set; }
        public DbSet<ProfesionalOficio> ProfesionalOficios { get; set; }
        public DbSet<Resena> Resenas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Suscripcion> Suscripciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            });

            // Configuración de Profesional
            modelBuilder.Entity<Profesional>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CalificacionPromedio).HasColumnType("decimal(3,2)");
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Profesionales)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Oficio
            modelBuilder.Entity<Oficio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            });

            // Configuración de ProfesionalOficio (tabla intermedia)
            modelBuilder.Entity<ProfesionalOficio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PrecioHora).HasColumnType("decimal(10,2)");
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.ProfesionalOficios)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Oficio)
                      .WithMany(o => o.ProfesionalOficios)
                      .HasForeignKey(e => e.OficioId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Resena
            modelBuilder.Entity<Resena>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.Resenas)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Resenas)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Certificado
            modelBuilder.Entity<Certificado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.Certificados)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Suscripcion
            modelBuilder.Entity<Suscripcion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.Suscripciones)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Datos iniciales
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Oficios iniciales
            modelBuilder.Entity<Oficio>().HasData(
                new Oficio { Id = 1, Nombre = "Plomería", Descripcion = "Reparación y mantenimiento de sistemas de plomería", Icono = "🔧", Color = "#3498db", FechaCreacion = new DateTime(2024, 1, 1) },
                new Oficio { Id = 2, Nombre = "Electricidad", Descripcion = "Instalaciones y reparaciones eléctricas", Icono = "⚡", Color = "#f39c12", FechaCreacion = new DateTime(2024, 1, 1) },
                new Oficio { Id = 3, Nombre = "Carpintería", Descripcion = "Trabajos en madera y muebles", Icono = "🪚", Color = "#8e44ad", FechaCreacion = new DateTime(2024, 1, 1) },
                new Oficio { Id = 4, Nombre = "Pintura", Descripcion = "Pintura de interiores y exteriores", Icono = "🎨", Color = "#e74c3c", FechaCreacion = new DateTime(2024, 1, 1) },
                new Oficio { Id = 5, Nombre = "Jardinería", Descripcion = "Mantenimiento de jardines y espacios verdes", Icono = "🌱", Color = "#27ae60", FechaCreacion = new DateTime(2024, 1, 1) },
                new Oficio { Id = 6, Nombre = "Limpieza", Descripcion = "Servicios de limpieza doméstica y comercial", Icono = "🧹", Color = "#16a085", FechaCreacion = new DateTime(2024, 1, 1) }
            );
        }
    }
}
