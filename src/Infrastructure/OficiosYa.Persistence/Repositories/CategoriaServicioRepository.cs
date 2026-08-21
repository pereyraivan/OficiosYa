using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;

namespace OficiosYa.Persistence.Repositories
{
    public class CategoriaServicioRepository : Repository<CategoriaServicio>, ICategoriaServicioRepository
    {
        public CategoriaServicioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<CategoriaServicio>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.TipoServicio)
                .OrderBy(c => c.TipoServicio!.Nombre)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoriaServicio>> GetByTipoAsync(string tipo, bool soloActivos = true)
        {
            var query = _dbSet
                .Include(c => c.TipoServicio)
                .Where(c => c.TipoServicio.Nombre.ToLower() == tipo.ToLower());

            if (soloActivos)
            {
                query = query.Where(c => c.Activo);
            }

            return await query
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoriaServicio>> GetActivosAsync()
        {
            return await _dbSet
                .Include(c => c.TipoServicio)
                .Where(c => c.Activo)
                .OrderBy(c => c.TipoServicio.Nombre)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<bool> NombreExistsAsync(string nombre, string tipo, int? excludeId = null)
        {
            var query = _dbSet
                .Include(c => c.TipoServicio)
                .Where(c => 
                    c.Nombre.ToLower() == nombre.ToLower() && 
                    c.TipoServicio.Nombre.ToLower() == tipo.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}

