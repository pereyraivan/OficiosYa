using Microsoft.EntityFrameworkCore;
using OficiosYa.Web.Data;
using OficiosYa.Web.Interfaces;
using OficiosYa.Web.Models;
using OficiosYa.Web.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace OficiosYa.Web.Services
{
    public class ProfesionalService : IProfesionalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ProfesionalService(
            ApplicationDbContext context,
            IProfesionalRepository profesionalRepository,
            IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _profesionalRepository = profesionalRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ProfesionalResponse?> RegistrarProfesionalAsync(RegistrarProfesionalRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Verificar si el email ya existe
                var usuarioExistente = await _usuarioRepository.ObtenerPorEmailAsync(request.Email);
                if (usuarioExistente != null)
                {
                    return null;
                }

                // Crear el usuario
                var usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Email = request.Email,
                    PasswordHash = HashPassword(request.Password),
                    Telefono = request.Telefono,
                    Direccion = request.Direccion,
                    Ciudad = request.Ciudad,
                    Provincia = request.Provincia,
                    CodigoPostal = request.CodigoPostal,
                    FechaRegistro = DateTime.UtcNow,
                    EmailVerificado = false,
                    Activo = true
                };

                await _usuarioRepository.AgregarAsync(usuario);
                await _context.SaveChangesAsync();

                // Crear el profesional
                var profesional = new Profesional
                {
                    UsuarioId = usuario.Id,
                    Descripcion = request.Descripcion,
                    FotoPerfil = request.FotoPerfil,
                    CalificacionPromedio = 0,
                    TotalResenas = 0,
                    Disponible = true,
                    FechaRegistro = DateTime.UtcNow,
                    Activo = true
                };

                await _profesionalRepository.AgregarAsync(profesional);
                await _context.SaveChangesAsync();

                // Agregar oficios
                foreach (var oficioNombre in request.Oficios)
                {
                    var oficio = await _context.Oficios.FirstOrDefaultAsync(o => o.Nombre == oficioNombre);
                    if (oficio == null)
                    {
                        oficio = new Oficio { Nombre = oficioNombre };
                        await _context.Oficios.AddAsync(oficio);
                        await _context.SaveChangesAsync();
                    }

                    var profesionalOficio = new ProfesionalOficio
                    {
                        ProfesionalId = profesional.Id,
                        OficioId = oficio.Id
                    };

                    await _context.ProfesionalOficios.AddAsync(profesionalOficio);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Retornar el profesional creado
                return await ObtenerProfesionalPorIdAsync(profesional.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ProfesionalResponse?> ObtenerProfesionalPorIdAsync(int id)
        {
            var profesional = await _profesionalRepository.ObtenerConOficiosAsync();
            var profesionalEncontrado = profesional.FirstOrDefault(p => p.Id == id);
            
            if (profesionalEncontrado == null)
                return null;

            return MapearAProfesionalResponse(profesionalEncontrado);
        }

        public async Task<ProfesionalResponse?> ObtenerProfesionalPorUsuarioIdAsync(int usuarioId)
        {
            var profesional = await _profesionalRepository.ObtenerPorUsuarioIdAsync(usuarioId);
            
            if (profesional == null)
                return null;

            return MapearAProfesionalResponse(profesional);
        }

        public async Task<List<ProfesionalResponse>> ObtenerProfesionalesDestacadosAsync()
        {
            var profesionales = await _profesionalRepository.ObtenerDestacadosAsync();
            return profesionales.Select(MapearAProfesionalResponse).ToList();
        }

        public async Task<List<ProfesionalResponse>> BuscarProfesionalesAsync(string oficio, string zona, int radio = 10)
        {
            var profesionales = await _profesionalRepository.BuscarPorOficioAsync(oficio);
            
            if (!string.IsNullOrEmpty(zona))
            {
                profesionales = profesionales.Where(p => 
                    p.Usuario.Ciudad != null && p.Usuario.Ciudad.Contains(zona, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return profesionales.Select(MapearAProfesionalResponse).ToList();
        }

        public async Task<List<ProfesionalResponse>> ObtenerProfesionalesPorOficioAsync(string oficio)
        {
            var profesionales = await _profesionalRepository.BuscarPorOficioAsync(oficio);
            return profesionales.Select(MapearAProfesionalResponse).ToList();
        }

        public async Task<List<ProfesionalResponse>> ObtenerProfesionalesCercaDeAsync(string ciudad, int radio = 10)
        {
            var profesionales = await _profesionalRepository.BuscarCercaDeAsync(ciudad, radio);
            return profesionales.Select(MapearAProfesionalResponse).ToList();
        }

        public async Task<bool> ActualizarDisponibilidadAsync(int id, bool disponible)
        {
            var profesional = await _profesionalRepository.ObtenerPorIdAsync(id);
            if (profesional == null)
                return false;

            profesional.Disponible = disponible;
            await _profesionalRepository.ActualizarAsync(profesional);
            return true;
        }

        public async Task<bool> ActualizarPerfilAsync(int id, string? descripcion, string? fotoPerfil)
        {
            var profesional = await _profesionalRepository.ObtenerPorIdAsync(id);
            if (profesional == null)
                return false;

            profesional.Descripcion = descripcion;
            profesional.FotoPerfil = fotoPerfil;
            await _profesionalRepository.ActualizarAsync(profesional);
            return true;
        }

        private ProfesionalResponse MapearAProfesionalResponse(Profesional profesional)
        {
            return new ProfesionalResponse
            {
                Id = profesional.Id,
                UsuarioId = profesional.UsuarioId,
                Descripcion = profesional.Descripcion,
                FotoPerfil = profesional.FotoPerfil,
                CalificacionPromedio = profesional.CalificacionPromedio,
                TotalResenas = profesional.TotalResenas,
                Disponible = profesional.Disponible,
                FechaRegistro = profesional.FechaRegistro,
                Activo = profesional.Activo,
                Usuario = new UsuarioResponse
                {
                    Id = profesional.Usuario.Id,
                    Nombre = $"{profesional.Usuario.Nombre} {profesional.Usuario.Apellido}",
                    Email = profesional.Usuario.Email,
                    Telefono = profesional.Usuario.Telefono,
                    FechaRegistro = profesional.Usuario.FechaRegistro.ToString("yyyy-MM-dd"),
                    EsProfesional = true,
                    Activo = profesional.Usuario.Activo
                },
                Oficios = profesional.ProfesionalOficios.Select(po => new OficioResponse
                {
                    Id = po.Oficio.Id,
                    Nombre = po.Oficio.Nombre,
                    Descripcion = po.Oficio.Descripcion
                }).ToList(),
                Certificados = profesional.Certificados.Select(c => new CertificadoResponse
                {
                    Id = c.Id,
                    ProfesionalId = c.ProfesionalId,
                    Nombre = c.Nombre,
                    Institucion = c.Institucion,
                    FechaEmision = c.FechaEmision,
                    FechaVencimiento = c.FechaVencimiento,
                    ArchivoUrl = c.ArchivoUrl
                }).ToList()
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
