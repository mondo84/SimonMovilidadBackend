using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAlertHub
    {
        Task SendLocationUpdate(SensorData dto);
        Task SendLowFuelAlert(string vehicleId, double remainingHours);
    }
}
