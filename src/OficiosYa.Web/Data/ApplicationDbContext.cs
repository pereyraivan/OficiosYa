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
        public DbSet<CategoriaServicio> CategoriaServicios { get; set; }
        public DbSet<ProfesionalCategoria> ProfesionalCategorias { get; set; }
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

            // Configuración de CategoriaServicio
            modelBuilder.Entity<CategoriaServicio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => new { e.Tipo, e.Nombre }).IsUnique();
            });

            // Configuración de ProfesionalCategoria (tabla intermedia)
            modelBuilder.Entity<ProfesionalCategoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.ProfesionalCategorias)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.CategoriaServicio)
                      .WithMany(c => c.ProfesionalCategorias)
                      .HasForeignKey(e => e.CategoriaServicioId)
                      .OnDelete(DeleteBehavior.Restrict);
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

            // Categorías de Servicio - Oficiantes
            modelBuilder.Entity<CategoriaServicio>().HasData(
                // Oficiantes
                new CategoriaServicio { Id = 1, Nombre = "Plomero", Descripcion = "Instalación y reparación de sistemas de plomería", Tipo = "Oficiante", Icono = "🔧", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 2, Nombre = "Electricista", Descripcion = "Instalaciones y reparaciones eléctricas", Tipo = "Oficiante", Icono = "⚡", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 3, Nombre = "Albañil", Descripcion = "Construcción y reparación de estructuras", Tipo = "Oficiante", Icono = "🧱", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 4, Nombre = "Gasista", Descripcion = "Instalación y reparación de sistemas de gas", Tipo = "Oficiante", Icono = "🔥", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 5, Nombre = "Jardinero", Descripcion = "Mantenimiento de jardines y espacios verdes", Tipo = "Oficiante", Icono = "🌿", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 6, Nombre = "Pintor", Descripcion = "Pintura de interiores y exteriores", Tipo = "Oficiante", Icono = "🎨", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 7, Nombre = "Cerrajero", Descripcion = "Apertura de cerraduras y copias de llaves", Tipo = "Oficiante", Icono = "🔒", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 8, Nombre = "Carpintero", Descripcion = "Trabajos en madera y muebles", Tipo = "Oficiante", Icono = "🪚", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 9, Nombre = "Herrero", Descripcion = "Trabajos en metal y herrería", Tipo = "Oficiante", Icono = "⚒️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 10, Nombre = "Técnico en Aire Acondicionado", Descripcion = "Instalación y reparación de sistemas de climatización", Tipo = "Oficiante", Icono = "❄️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 11, Nombre = "Técnico en Refrigeración", Descripcion = "Reparación de heladeras y freezers", Tipo = "Oficiante", Icono = "🧊", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 12, Nombre = "Limpieza", Descripcion = "Servicios de limpieza doméstica y comercial", Tipo = "Oficiante", Icono = "🧹", FechaCreacion = new DateTime(2024, 1, 1) },

                // Profesionales
                new CategoriaServicio { Id = 13, Nombre = "Abogado", Descripcion = "Servicios legales y asesoramiento jurídico", Tipo = "Profesional", Icono = "⚖️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 14, Nombre = "Contador", Descripcion = "Servicios contables y fiscales", Tipo = "Profesional", Icono = "📊", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 15, Nombre = "Médico", Descripcion = "Servicios de salud y atención médica", Tipo = "Profesional", Icono = "🩺", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 16, Nombre = "Psicólogo", Descripcion = "Servicios de salud mental y terapia", Tipo = "Profesional", Icono = "🧠", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 17, Nombre = "Dentista", Descripcion = "Servicios de salud dental", Tipo = "Profesional", Icono = "🦷", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 18, Nombre = "Veterinario", Descripcion = "Servicios de salud animal", Tipo = "Profesional", Icono = "🐕", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 19, Nombre = "Arquitecto", Descripcion = "Diseño arquitectónico y planificación", Tipo = "Profesional", Icono = "🏗️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 20, Nombre = "Ingeniero", Descripcion = "Servicios de ingeniería y consultoría técnica", Tipo = "Profesional", Icono = "⚙️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 21, Nombre = "Diseñador Gráfico", Descripcion = "Servicios de diseño visual y comunicación", Tipo = "Profesional", Icono = "🎨", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 22, Nombre = "Traductor", Descripcion = "Servicios de traducción e interpretación", Tipo = "Profesional", Icono = "🌐", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 23, Nombre = "Coach", Descripcion = "Servicios de coaching y desarrollo personal", Tipo = "Profesional", Icono = "🎯", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 24, Nombre = "Nutricionista", Descripcion = "Servicios de nutrición y alimentación saludable", Tipo = "Profesional", Icono = "🥗", FechaCreacion = new DateTime(2024, 1, 1) }
            );
        }
    }
}
