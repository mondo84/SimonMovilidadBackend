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

        public async Task AddAsync(SensorData entity)
        {
            await _context.SensorData.AddAsync(entity);
        }

        public async Task<List<SensorData>> GetSensorDataListAsync(string vehicleId)
        {
            var resp = await _context.SensorData.Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(or => or.Timestamp)
                .Take(10)
                .ToListAsync();
  
            return resp;
        }
    }
}
