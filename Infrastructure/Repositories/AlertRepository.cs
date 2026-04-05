using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AlertRepository(AppDbContext context) : IAlertRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(Alerts entity)
        {
            await _context.Alerts.AddAsync(entity);
        }

        public async Task<Alerts?> GetAlertByIdAsync(int alarmId)
        {
            return await _context.Alerts.FirstOrDefaultAsync(row => row.Id == alarmId);
        }

        public async Task<List<Alerts>> GetAlertListAsync(string? vehicleId, DateOnly date, bool showInactive)
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

            IQueryable<Alerts> query = _context.Alerts
                .AsNoTracking()
                .Where(w => w.CreatedAt >= startUtc && w.CreatedAt < endUtc);

            if (!string.IsNullOrWhiteSpace(vehicleId))
                query = query.Where(w => w.VehicleId == vehicleId);

            if (!showInactive)
                query = query.Where(w => w.Active);

            return await query.ToListAsync();
        }

        public async Task<Alerts?> GetLastAlertByVehicle(string vehicleId)
        {
            return await _context.Alerts
            .Where(a => a.VehicleId == vehicleId)
            .OrderByDescending(a => a.CreatedAt)
            .FirstOrDefaultAsync();
        }
    }
}
