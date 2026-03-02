using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;

namespace OficiosYa.Persistence.Repositories
{
    public class ProfesionalRepository : Repository<Profesional>, IProfesionalRepository
    {
        public ProfesionalRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Profesional>> GetByOficioAsync(int oficioId)
        {
            return await _dbSet
                .Where(p => p.ProfesionalOficios.Any(po => po.OficioId == oficioId && po.Activo))
                .ToListAsync();
        }

        public async Task<IEnumerable<Profesional>> GetByCategoriaAsync(int categoriaId)
        {
            return await _dbSet
                .Where(p => p.ProfesionalCategorias.Any(pc => pc.CategoriaServicioId == categoriaId && pc.Activo))
                .ToListAsync();
        }

        public async Task<IEnumerable<Profesional>> GetDisponiblesAsync()
        {
            return await _dbSet
                .Where(p => p.Disponible && p.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Profesional>> BuscarAsync(string? termino, int? oficioId, int? categoriaId, decimal? precioMin, decimal? precioMax, string? localidad, string? provincia)
        {
            var query = _dbSet
                .Include(p => p.ZonasCobertura)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(termino))
            {
                query = query.Where(p => p.Descripcion.Contains(termino) ||
                                       (p.Usuario != null && (p.Usuario.Nombre.Contains(termino) || p.Usuario.Apellido.Contains(termino))));
            }

            if (oficioId.HasValue)
            {
                query = query.Where(p => p.ProfesionalOficios.Any(po => po.OficioId == oficioId.Value && po.Activo));
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(p => p.ProfesionalCategorias.Any(pc => pc.CategoriaServicioId == categoriaId.Value && pc.Activo));
            }

            if (precioMin.HasValue)
            {
                query = query.Where(p => p.ProfesionalOficios.Any(po => po.PrecioHora >= precioMin.Value && po.Activo));
            }

            if (precioMax.HasValue)
            {
                query = query.Where(p => p.ProfesionalOficios.Any(po => po.PrecioHora <= precioMax.Value && po.Activo));
            }

            // Filtrar por localidad o provincia usando zonas de cobertura
            if (!string.IsNullOrWhiteSpace(localidad) || !string.IsNullOrWhiteSpace(provincia))
            {
                query = query.Where(p => p.ZonasCobertura.Any(z => z.Activo && 
                    (string.IsNullOrWhiteSpace(localidad) || z.Localidad.Contains(localidad, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(provincia) || z.Provincia.Contains(provincia, StringComparison.OrdinalIgnoreCase))));
            }

            return await query
                .Where(p => p.Disponible && p.Activo)
                .ToListAsync();
        }

        public async Task<Profesional?> GetWithOficiosAsync(int id)
        {
            return await _dbSet
                .Include(p => p.ProfesionalOficios)
                    .ThenInclude(po => po.Oficio)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profesional?> GetWithCategoriasAsync(int id)
        {
            return await _dbSet
                .Include(p => p.ProfesionalCategorias)
                    .ThenInclude(pc => pc.CategoriaServicio)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profesional?> GetWithResenasAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Resenas)
                    .ThenInclude(r => r.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profesional?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Profesional>> GetTopRatedAsync(int cantidad = 10)
        {
            return await _dbSet
                .Where(p => p.Activo && p.TotalResenas > 0)
                .OrderByDescending(p => p.CalificacionPromedio)
                .ThenByDescending(p => p.TotalResenas)
                .Take(cantidad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Profesional>> GetNearbyAsync(double latitud, double longitud, double radioKm = 10)
        {
            // Implementación básica - en un proyecto real usarías geolocalización
            return await _dbSet
                .Where(p => p.Disponible && p.Activo)
                .ToListAsync();
        }

        public async Task AsociarCategoriasAsync(int profesionalId, IEnumerable<string> nombresCategorias)
        {
            // Obtener las categorías por nombre
            var categorias = await _context.CategoriaServicios
                .Where(c => nombresCategorias.Contains(c.Nombre) && c.Activo)
                .ToListAsync();

            if (categorias.Any())
            {
                var profesionalCategorias = categorias.Select(categoria => new ProfesionalCategoria
                {
                    ProfesionalId = profesionalId,
                    CategoriaServicioId = categoria.Id,
                    FechaAsignacion = DateTime.UtcNow,
                    Activo = true
                }).ToList();

                await _context.ProfesionalCategorias.AddRangeAsync(profesionalCategorias);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AsociarZonasCoberturaAsync(int profesionalId, IEnumerable<ZonaCobertura> zonas)
        {
            if (zonas != null && zonas.Any())
            {
                foreach (var zona in zonas)
                {
                    zona.ProfesionalId = profesionalId;
                    zona.FechaRegistro = DateTime.UtcNow;
                    zona.Activo = true;
                }

                await _context.ZonasCobertura.AddRangeAsync(zonas);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Profesional>> BuscarPorLocalidadAsync(string? localidad, string? provincia)
        {
            var query = _dbSet
                .Include(p => p.ZonasCobertura)
                .Where(p => p.Disponible && p.Activo && p.ZonasCobertura.Any(z => z.Activo));

            if (!string.IsNullOrWhiteSpace(localidad))
            {
                query = query.Where(p => p.ZonasCobertura.Any(z => 
                    z.Localidad.Contains(localidad, StringComparison.OrdinalIgnoreCase) && z.Activo));
            }

            if (!string.IsNullOrWhiteSpace(provincia))
            {
                query = query.Where(p => p.ZonasCobertura.Any(z => 
                    z.Provincia.Contains(provincia, StringComparison.OrdinalIgnoreCase) && z.Activo));
            }

            return await query.ToListAsync();
        }
    }
}
