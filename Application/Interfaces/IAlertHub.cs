using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAlertHub
    {
        Task SendLocationUpdate(string vehicleId, double lat, double lng, DateTime timestamp);
        Task SendLowFuelAlert(string vehicleId, double remainingHours);
    }
}
