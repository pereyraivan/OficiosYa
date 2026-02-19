using Microsoft.EntityFrameworkCore;
using OficiosYa.Domain.Entities;
using OficiosYa.Domain.Interfaces;
using OficiosYa.Persistence.Context;

namespace OficiosYa.Persistence.Repositories
{
    public class PlanSuscripcionRepository : Repository<PlanSuscripcion>, IPlanSuscripcionRepository
    {
        public PlanSuscripcionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<PlanSuscripcion>> GetAllActivosAsync()
        {
            return await _dbSet
                .Where(p => p.Activo)
                .OrderBy(p => p.PrecioMensual)
                .ToListAsync();
        }

        public async Task<PlanSuscripcion?> GetByNombreAsync(string nombre)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Nombre.ToLower() == nombre.ToLower() && p.Activo);
        }
    }
}

