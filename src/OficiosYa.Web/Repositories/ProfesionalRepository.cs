using Microsoft.EntityFrameworkCore;
using OficiosYa.Web.Data;
using OficiosYa.Web.Interfaces;
using OficiosYa.Web.Models;

namespace OficiosYa.Web.Repositories
{
    public class ProfesionalRepository : Repository<Profesional>, IProfesionalRepository
    {
        public ProfesionalRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Profesional?> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.ProfesionalOficios)
                    .ThenInclude(po => po.Oficio)
                .Include(p => p.Certificados)
                .Include(p => p.Resenas)
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.Activo);
        }

        public async Task<List<Profesional>> ObtenerDestacadosAsync()
        {
            return await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.ProfesionalOficios)
                    .ThenInclude(po => po.Oficio)
                .Include(p => p.Certificados)
                .Include(p => p.Resenas)
                .Where(p => p.Activo && p.Disponible && p.CalificacionPromedio >= 4.0m)
                .OrderByDescending(p => p.CalificacionPromedio)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<Profesional>> BuscarPorOficioAsync(string oficio)
        {
            return await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.ProfesionalOficios)
                    .ThenInclude(po => po.Oficio)
                .Include(p => p.Certificados)
                .Include(p => p.Resenas)
                .Where(p => p.Activo && p.Disponible && 
                           p.ProfesionalOficios.Any(po => po.Oficio.Nombre.Contains(oficio)))
                .OrderByDescending(p => p.CalificacionPromedio)
                .ToListAsync();
        }

        public async Task<List<Profesional>> BuscarCercaDeAsync(string ciudad, int radio = 10)
        {
            return await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.ProfesionalOficios)
                    .ThenInclude(po => po.Oficio)
                .Include(p => p.Certificados)
                .Include(p => p.Resenas)
                .Where(p => p.Activo && p.Disponible && 
                           p.Usuario.Ciudad != null && p.Usuario.Ciudad.Contains(ciudad))
                .OrderByDescending(p => p.CalificacionPromedio)
                .ToListAsync();
        }

        public async Task<List<Profesional>> ObtenerConOficiosAsync()
        {
            return await _context.Profesionales
                .Include(p => p.Usuario)
                .Include(p => p.ProfesionalOficios)
                    .ThenInclude(po => po.Oficio)
                .Include(p => p.Certificados)
                .Include(p => p.Resenas)
                .Where(p => p.Activo)
                .ToListAsync();
        }
    }
}
