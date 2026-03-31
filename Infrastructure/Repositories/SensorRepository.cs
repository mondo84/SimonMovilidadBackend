using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SensorRepository(AppDbContext context) : ISensorRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAlertAsync(Alerts alert)
        {
            await _context.Alerts.AddAsync(alert);
        }

        public async Task AddAsync(SensorData entity)
        {
            await _context.SensorData.AddAsync(entity);
        }

        public async Task<List<Alerts>> GetAlertListAsync(string vehicleId, DateTime date, bool showInactive)
        {
            IQueryable<Alerts> query = _context.Alerts
                .AsNoTracking()
                .Where(w => w.CreatedAt.Date == date.Date);

            if (!string.IsNullOrWhiteSpace(vehicleId))
                query = query.Where(w => w.VehicleId == vehicleId);

            if (!showInactive)
                query = query.Where(w => w.Active);

            return await query.ToListAsync();
        }

        public async Task<List<SensorData>> GetAllAsync(bool showInactive)
        {
            var query = _context.SensorData
                    .Where(w => w.VehicleId != null)
                    .AsNoTracking();

            if (!showInactive)
                query = query.Where(u => u.Active);

            return await query.ToListAsync();
        }

        public async Task<List<SensorData>> GetSensorDataListAsync(string vehicleId)
        {
            var resp = await _context.SensorData.Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(or => or.Timestamp)
                .Take(10)
                .ToListAsync();
  
            return resp;
        }

        public async Task<List<SensorData>> TodaysSensorDataHistory(int takeNumber)
        {
            // Creacion mayor a la media noche de hoy, y menor a la de mañana.
            return await _context.SensorData
                .Where(w => w.Active)
                .OrderByDescending(w => w.Timestamp)
                .Take(takeNumber)
                .OrderBy(w => w.Timestamp)  // reordenar para gráfico cronológico
                .ToListAsync();
        }
    }
}
