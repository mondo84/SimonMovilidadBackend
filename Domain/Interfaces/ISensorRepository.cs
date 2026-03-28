using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISensorRepository
    {
        Task AddAsync(SensorData user);
        Task<List<SensorData>> GetSensorDataListAsync(string vehicleId);
    }
}
