using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISensorRepository
    {
        Task AddAsync(SensorData user);
        Task<List<SensorData>> GetSensorDataListAsync(string vehicleId);
        Task<List<SensorData>> TodaysSensorDataHistory(int takeNumber);
        Task<List<SensorData>> GetAllAsync(DateOnly date, bool showInactive);
    }
}
