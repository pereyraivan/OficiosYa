using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;

namespace OficiosYa.Persistence.Repositories
{
    public class TipoServicioRepository : Repository<TipoServicio>, ITipoServicioRepository
    {
        public TipoServicioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<TipoServicio?> GetByNombreAsync(string nombre)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.Nombre.ToLower() == nombre.ToLower() && t.Activo);
        }

        public async Task<List<TipoServicio>> GetAllActivosAsync()
        {
            return await _dbSet
                .Where(t => t.Activo)
                .OrderBy(t => t.Nombre)
                .ToListAsync();
        }
    }
}

