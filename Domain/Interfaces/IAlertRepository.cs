using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAlertRepository
    {
        Task AddAsync(Alerts user);
        Task<List<Alerts>> GetAlertListAsync(string? vehicleId, DateOnly date, bool showInactive);
    }
}
