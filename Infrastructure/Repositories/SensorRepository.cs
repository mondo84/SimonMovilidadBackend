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

        public async Task<List<SensorData>> GetAllAsync(DateOnly date, bool showInactive)
        {
            var start = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                0, 0, 0,
                DateTimeKind.Local
            );
            var end = start.AddDays(1);

            var startUtc = start.ToUniversalTime();
            var endUtc = end.ToUniversalTime();

            var query = _context.SensorData
                    .Where(w => w.VehicleId != null 
                    && w.Timestamp >= startUtc && w.Timestamp < endUtc)
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
            var todayUtc = DateTime.UtcNow.Date;
            var tomorrowUtc = todayUtc.AddDays(1);

            // Creacion mayor a la media noche de hoy, y menor a la de mañana.
            return await _context.SensorData
                .Where(w => w.Active && w.Timestamp >= todayUtc && w.Timestamp < tomorrowUtc)
                .OrderByDescending(w => w.Timestamp)
                .Take(takeNumber)
                .OrderBy(w => w.Timestamp)  // reordenar para gráfico cronológico
                .ToListAsync();
        }
    }
}
