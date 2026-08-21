using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;

namespace OficiosYa.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
        public DbSet<CategoriaServicio> CategoriaServicios { get; set; }
        public DbSet<ProfesionalCategoria> ProfesionalCategorias { get; set; }
        public DbSet<Resena> Resenas { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Suscripcion> Suscripciones { get; set; }
        public DbSet<PlanSuscripcion> PlanesSuscripcion { get; set; }
        public DbSet<ZonaCobertura> ZonasCobertura { get; set; }
        public DbSet<ImagenTrabajo> ImagenesTrabajo { get; set; }
        public DbSet<TrabajoPublicado> TrabajosPublicados { get; set; }
        public DbSet<TrabajoCategoria> TrabajoCategorias { get; set; }
        public DbSet<ContactoProfesional> ContactosProfesional { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraci?n de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            });

            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TokenHash).IsRequired().HasMaxLength(64);
                entity.HasIndex(e => e.TokenHash);
                entity.HasIndex(e => e.UsuarioId);
                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuraci?n de Profesional
            modelBuilder.Entity<Profesional>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CalificacionPromedio).HasColumnType("decimal(3,2)");
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Profesionales)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Resena (UsuarioId opcional para reseñas de visitantes)
            modelBuilder.Entity<Resena>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Calificacion).HasColumnName("Puntuacion");
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.Resenas)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Resenas)
                      .HasForeignKey(e => e.UsuarioId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de ContactoProfesional (registro de clics en "Contactar" para planes por contacto)
            modelBuilder.Entity<ContactoProfesional>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TipoContacto).HasMaxLength(20);
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.ContactosRecibidos)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UsuarioId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
                entity.ToTable("ContactosProfesional", "dbo");
            });

            // Configuraci?n de Certificado
            modelBuilder.Entity<Certificado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.Certificados)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de TipoServicio (especificando esquema dbo explícitamente)
            modelBuilder.Entity<TipoServicio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");
                entity.Property(e => e.Descripcion).HasMaxLength(500).HasColumnType("nvarchar");
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.ToTable("TipoServicios", "dbo");
            });

            // Configuración de CategoriaServicio (especificando esquema dbo explícitamente)
            modelBuilder.Entity<CategoriaServicio>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100).HasColumnType("nvarchar");
                entity.Property(e => e.Descripcion).HasMaxLength(500).HasColumnType("nvarchar");
                entity.Property(e => e.TipoServicioId).IsRequired();
                entity.Property(e => e.Grupo).HasMaxLength(100).HasColumnType("nvarchar");
                entity.Property(e => e.Icono).HasMaxLength(10).HasColumnType("nvarchar");
                entity.HasIndex(e => new { e.TipoServicioId, e.Nombre }).IsUnique();
                entity.ToTable("CategoriaServicios", "dbo");
                entity.HasOne(e => e.TipoServicio)
                      .WithMany(t => t.CategoriaServicios)
                      .HasForeignKey(e => e.TipoServicioId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuraci?n de ProfesionalCategoria (tabla intermedia)
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
                entity.Property(e => e.Precio).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TipoSuscripcion).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MetodoPago).HasMaxLength(50);
                entity.Property(e => e.CodigoPago).HasMaxLength(100);
                entity.Property(e => e.PagoConfirmado).IsRequired();
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.Suscripciones)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.PlanSuscripcion)
                      .WithMany(p => p.Suscripciones)
                      .HasForeignKey(e => e.PlanSuscripcionId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuraci?n de PlanSuscripcion
            modelBuilder.Entity<PlanSuscripcion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PrecioMensual).HasColumnType("decimal(10,2)");
                entity.Property(e => e.PrecioTrimestral).HasColumnType("decimal(10,2)");
                entity.Property(e => e.PrecioSemestral).HasColumnType("decimal(10,2)");
                entity.Property(e => e.PrecioAnual).HasColumnType("decimal(10,2)");
                entity.Property(e => e.DescuentoTrimestral).HasColumnType("decimal(5,2)");
                entity.Property(e => e.DescuentoSemestral).HasColumnType("decimal(5,2)");
                entity.Property(e => e.DescuentoAnual).HasColumnType("decimal(5,2)");
                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            // Configuraci?n de ZonaCobertura
            modelBuilder.Entity<ZonaCobertura>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Latitud).HasColumnType("decimal(10,8)");
                entity.Property(e => e.Longitud).HasColumnType("decimal(11,8)");
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.ZonasCobertura)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de ImagenTrabajo (portfolio / fotos de trabajos realizados)
            modelBuilder.Entity<ImagenTrabajo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UrlImagen).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Titulo).HasMaxLength(200);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.HasOne(e => e.Profesional)
                      .WithMany(p => p.ImagenesTrabajo)
                      .HasForeignKey(e => e.ProfesionalId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Trabajo publicado (empleo que un cliente publica para que se postulen)
            modelBuilder.Entity<TrabajoPublicado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Estado).HasMaxLength(20);
                entity.ToTable("TrabajosPublicados", "dbo");
            });

            // TrabajoCategoria: categorías de servicio que requiere el trabajo
            modelBuilder.Entity<TrabajoCategoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.TrabajoPublicado)
                      .WithMany(t => t.TrabajoCategorias)
                      .HasForeignKey(e => e.TrabajoPublicadoId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.CategoriaServicio)
                      .WithMany()
                      .HasForeignKey(e => e.CategoriaServicioId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.TrabajoPublicadoId, e.CategoriaServicioId }).IsUnique();
                entity.ToTable("TrabajoCategorias", "dbo");
            });

            // Datos iniciales
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Tipos de Servicio
            modelBuilder.Entity<TipoServicio>().HasData(
                new TipoServicio { Id = 1, Nombre = "Oficiante", Descripcion = "Servicios de oficios y trabajos manuales", Activo = true, FechaCreacion = new DateTime(2024, 1, 1) },
                new TipoServicio { Id = 2, Nombre = "Profesional", Descripcion = "Servicios profesionales y especializados", Activo = true, FechaCreacion = new DateTime(2024, 1, 1) }
            );

            // Categorías de Servicio - Oficiantes (TipoServicioId = 1)
            modelBuilder.Entity<CategoriaServicio>().HasData(
                // Oficiantes - Construcción
                new CategoriaServicio { Id = 1, Nombre = "Plomero", Descripcion = "Instalación y reparación de sistemas de plomería", TipoServicioId = 1, Grupo = "Construcción", Icono = "🚰", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 2, Nombre = "Electricista", Descripcion = "Instalaciones y reparaciones eléctricas", TipoServicioId = 1, Grupo = "Construcción", Icono = "⚡", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 3, Nombre = "Albañil", Descripcion = "Construcción y reparación de estructuras", TipoServicioId = 1, Grupo = "Construcción", Icono = "🧱", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 4, Nombre = "Gasista", Descripcion = "Instalación y reparación de sistemas de gas", TipoServicioId = 1, Grupo = "Construcción", Icono = "🔥", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 6, Nombre = "Pintor", Descripcion = "Pintura de interiores y exteriores", TipoServicioId = 1, Grupo = "Construcción", Icono = "🎨", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 8, Nombre = "Carpintero", Descripcion = "Trabajos en madera y muebles", TipoServicioId = 1, Grupo = "Construcción", Icono = "🪵", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 9, Nombre = "Herrero", Descripcion = "Trabajos en metal y herrería", TipoServicioId = 1, Grupo = "Construcción", Icono = "⚒️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 25, Nombre = "Durlockero", Descripcion = "Instalación de construcción en seco (durlock)", TipoServicioId = 1, Grupo = "Construcción", Icono = "🏗️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 26, Nombre = "Construcción en Seco", Descripcion = "Trabajos en construcción en seco y durlock", TipoServicioId = 1, Grupo = "Construcción", Icono = "🏗️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 27, Nombre = "Techista", Descripcion = "Reparación y construcción de techos", TipoServicioId = 1, Grupo = "Construcción", Icono = "🏠", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 28, Nombre = "Soldador", Descripcion = "Trabajos de soldadura y metalurgia", TipoServicioId = 1, Grupo = "Construcción", Icono = "🔧", FechaCreacion = new DateTime(2024, 1, 1) },
                
                // Oficiantes - Hogar y Mantenimiento
                new CategoriaServicio { Id = 5, Nombre = "Jardinero", Descripcion = "Mantenimiento de jardines y espacios verdes", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🌿", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 7, Nombre = "Cerrajero", Descripcion = "Apertura de cerraduras y copias de llaves", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🔒", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 10, Nombre = "Técnico en Aire Acondicionado", Descripcion = "Instalación y reparación de sistemas de climatización", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "❄️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 11, Nombre = "Técnico en Refrigeración", Descripcion = "Reparación de heladeras y freezers", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🧊", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 12, Nombre = "Limpieza", Descripcion = "Servicios de limpieza doméstica y comercial", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🧹", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 29, Nombre = "Tapicero", Descripcion = "Reparación y tapizado de muebles", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🪑", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 30, Nombre = "Marmolista", Descripcion = "Trabajos en mármol y granito", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🪨", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 31, Nombre = "Vidriero", Descripcion = "Instalación y reparación de vidrios", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🪟", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 32, Nombre = "Colchonero", Descripcion = "Reparación y restauración de colchones", TipoServicioId = 1, Grupo = "Hogar y Mantenimiento", Icono = "🛏️", FechaCreacion = new DateTime(2024, 1, 1) },

                // Profesionales - Salud
                new CategoriaServicio { Id = 13, Nombre = "Médico", Descripcion = "Servicios de salud y atención médica", TipoServicioId = 2, Grupo = "Salud", Icono = "👨‍⚕️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 14, Nombre = "Psicólogo", Descripcion = "Servicios de salud mental y terapia", TipoServicioId = 2, Grupo = "Salud", Icono = "🧠", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 15, Nombre = "Dentista", Descripcion = "Servicios de salud dental", TipoServicioId = 2, Grupo = "Salud", Icono = "🦷", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 16, Nombre = "Veterinario", Descripcion = "Servicios de salud animal", TipoServicioId = 2, Grupo = "Salud", Icono = "🐾", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 17, Nombre = "Nutricionista", Descripcion = "Servicios de nutrición y alimentación saludable", TipoServicioId = 2, Grupo = "Salud", Icono = "🥗", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 33, Nombre = "Fonoaudiólogo", Descripcion = "Servicios de fonoaudiología y terapia del lenguaje", TipoServicioId = 2, Grupo = "Salud", Icono = "🗣️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 34, Nombre = "Kinesiólogo", Descripcion = "Servicios de kinesiología y fisioterapia", TipoServicioId = 2, Grupo = "Salud", Icono = "💪", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 35, Nombre = "Enfermero", Descripcion = "Servicios de enfermería y cuidados de salud", TipoServicioId = 2, Grupo = "Salud", Icono = "👩‍⚕️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 36, Nombre = "Terapeuta Ocupacional", Descripcion = "Servicios de terapia ocupacional", TipoServicioId = 2, Grupo = "Salud", Icono = "🤲", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 37, Nombre = "Optometrista", Descripcion = "Servicios de salud visual y optometría", TipoServicioId = 2, Grupo = "Salud", Icono = "👓", FechaCreacion = new DateTime(2024, 1, 1) },

                // Profesionales - Legal y Financiero
                new CategoriaServicio { Id = 18, Nombre = "Abogado", Descripcion = "Servicios legales y asesoramiento jurídico", TipoServicioId = 2, Grupo = "Legal y Financiero", Icono = "⚖️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 19, Nombre = "Contador", Descripcion = "Servicios contables y fiscales", TipoServicioId = 2, Grupo = "Legal y Financiero", Icono = "📊", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 38, Nombre = "Martillero Público", Descripcion = "Servicios de remates y subastas", TipoServicioId = 2, Grupo = "Legal y Financiero", Icono = "🔨", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 39, Nombre = "Escribano", Descripcion = "Servicios notariales y certificaciones", TipoServicioId = 2, Grupo = "Legal y Financiero", Icono = "📝", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 40, Nombre = "Asesor Financiero", Descripcion = "Asesoramiento financiero y inversiones", TipoServicioId = 2, Grupo = "Legal y Financiero", Icono = "💰", FechaCreacion = new DateTime(2024, 1, 1) },

                // Profesionales - Diseño y Arquitectura
                new CategoriaServicio { Id = 20, Nombre = "Arquitecto", Descripcion = "Diseño arquitectónico y planificación", TipoServicioId = 2, Grupo = "Diseño y Arquitectura", Icono = "🏛️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 21, Nombre = "Ingeniero", Descripcion = "Servicios de ingeniería y consultoría técnica", TipoServicioId = 2, Grupo = "Diseño y Arquitectura", Icono = "🔧", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 22, Nombre = "Diseñador Gráfico", Descripcion = "Servicios de diseño visual y comunicación", TipoServicioId = 2, Grupo = "Diseño y Arquitectura", Icono = "🎨", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 41, Nombre = "Diseñador de Interiores", Descripcion = "Diseño de interiores y decoración", TipoServicioId = 2, Grupo = "Diseño y Arquitectura", Icono = "🏠", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 42, Nombre = "Diseñador Web", Descripcion = "Diseño y desarrollo web", TipoServicioId = 2, Grupo = "Diseño y Arquitectura", Icono = "💻", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 43, Nombre = "Fotógrafo", Descripcion = "Servicios de fotografía profesional", TipoServicioId = 2, Grupo = "Diseño y Arquitectura", Icono = "📷", FechaCreacion = new DateTime(2024, 1, 1) },

                // Profesionales - Tecnología e IT
                new CategoriaServicio { Id = 47, Nombre = "Programador", Descripcion = "Desarrollo de software y aplicaciones", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "💻", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 48, Nombre = "Desarrollador Web", Descripcion = "Desarrollo de aplicaciones y sitios web", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🌐", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 49, Nombre = "Desarrollador Mobile", Descripcion = "Desarrollo de aplicaciones móviles", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "📱", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 50, Nombre = "Tester/QA", Descripcion = "Testing y aseguramiento de calidad de software", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "✅", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 51, Nombre = "Analista de Sistemas", Descripcion = "Análisis y diseño de sistemas informáticos", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "📊", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 52, Nombre = "Especialista en Ciberseguridad", Descripcion = "Seguridad informática y protección de datos", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🔒", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 53, Nombre = "DevOps Engineer", Descripcion = "Automatización e infraestructura de desarrollo", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "⚙️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 54, Nombre = "Administrador de Sistemas", Descripcion = "Administración y mantenimiento de sistemas", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🖥️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 55, Nombre = "DBA", Descripcion = "Administración de bases de datos", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🗄️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 56, Nombre = "Arquitecto de Software", Descripcion = "Diseño arquitectónico de software y sistemas", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🏗️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 57, Nombre = "Product Manager", Descripcion = "Gestión de productos tecnológicos", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "📦", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 58, Nombre = "Scrum Master", Descripcion = "Gestión ágil de proyectos de software", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🔄", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 59, Nombre = "Data Scientist", Descripcion = "Análisis de datos y ciencia de datos", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "📈", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 60, Nombre = "Cloud Engineer", Descripcion = "Ingeniería y administración en la nube", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "☁️", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 61, Nombre = "UX/UI Designer", Descripcion = "Diseño de experiencia e interfaz de usuario", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🎨", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 62, Nombre = "Soporte Técnico IT", Descripcion = "Soporte técnico y helpdesk", TipoServicioId = 2, Grupo = "Tecnología e IT", Icono = "🛠️", FechaCreacion = new DateTime(2024, 1, 1) },

                // Profesionales - Otros Servicios
                new CategoriaServicio { Id = 23, Nombre = "Traductor", Descripcion = "Servicios de traducción e interpretación", TipoServicioId = 2, Grupo = "Otros Servicios", Icono = "🌍", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 24, Nombre = "Coach", Descripcion = "Servicios de coaching y desarrollo personal", TipoServicioId = 2, Grupo = "Otros Servicios", Icono = "🎯", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 44, Nombre = "Profesor Particular", Descripcion = "Clases particulares y tutorías", TipoServicioId = 2, Grupo = "Otros Servicios", Icono = "📚", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 45, Nombre = "Instructor de Fitness", Descripcion = "Entrenamiento personal y fitness", TipoServicioId = 2, Grupo = "Otros Servicios", Icono = "💪", FechaCreacion = new DateTime(2024, 1, 1) },
                new CategoriaServicio { Id = 46, Nombre = "Masajista", Descripcion = "Servicios de masajes y relajación", TipoServicioId = 2, Grupo = "Otros Servicios", Icono = "💆", FechaCreacion = new DateTime(2024, 1, 1) }
            );

            // Planes de Suscripción
            modelBuilder.Entity<PlanSuscripcion>().HasData(
                new PlanSuscripcion 
                { 
                    Id = 1, 
                    Nombre = "Basico",
                    Descripcion = "20 contactos por mes. Ver empleos publicados (sin postularse).",
                    PrecioMensual = 6900m,
                    PrecioTrimestral = 18630m, // 10% descuento
                    PrecioSemestral = 35190m, // 15% descuento
                    PrecioAnual = 66240m, // 20% descuento
                    DescuentoTrimestral = 10m,
                    DescuentoSemestral = 15m,
                    DescuentoAnual = 20m,
                    Activo = true,
                    FechaCreacion = new DateTime(2024, 1, 1)
                },
                new PlanSuscripcion 
                { 
                    Id = 2, 
                    Nombre = "Premium", 
                    Descripcion = "Todo ilimitado: contactos y postulaciones a empleos.",
                    PrecioMensual = 17900m,
                    PrecioTrimestral = 48330m, // 10% descuento
                    PrecioSemestral = 91290m, // 15% descuento
                    PrecioAnual = 171840m, // 20% descuento
                    DescuentoTrimestral = 10m,
                    DescuentoSemestral = 15m,
                    DescuentoAnual = 20m,
                    Activo = true,
                    FechaCreacion = new DateTime(2024, 1, 1)
                },
                new PlanSuscripcion 
                { 
                    Id = 3, 
                    Nombre = "Pro", 
                    Descripcion = "Contactos ilimitados y hasta 5 postulaciones por período.",
                    PrecioMensual = 11900m,
                    PrecioTrimestral = 32130m, // 10% descuento
                    PrecioSemestral = 60690m, // 15% descuento
                    PrecioAnual = 114240m, // 20% descuento
                    DescuentoTrimestral = 10m,
                    DescuentoSemestral = 15m,
                    DescuentoAnual = 20m,
                    Activo = true,
                    FechaCreacion = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}
