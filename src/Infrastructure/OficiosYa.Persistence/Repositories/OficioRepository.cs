using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;

namespace OficiosYa.Persistence.Repositories
{
    public class OficioRepository : Repository<Oficio>, IOficioRepository
    {
        public OficioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Oficio>> GetActivosAsync()
        {
            return await _dbSet
                .Where(o => o.Activo)
                .OrderBy(o => o.Nombre)
                .ToListAsync();
        }

        public async Task<bool> NombreExistsAsync(string nombre, int? excludeId = null)
        {
            var query = _dbSet.Where(o => o.Nombre.ToLower() == nombre.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(o => o.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}

