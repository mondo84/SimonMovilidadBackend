using Application.Interfaces;
using IotApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IotApp.RealTime
{
    public class SignalrNotifier(IHubContext<AlertsHub> hub) : IAlertHub
    {
        private readonly IHubContext<AlertsHub> _hub = hub;

        public async Task SendLocationUpdate(string vehicleId, double lat, double lng, DateTime timestamp)
        {
            await _hub.Clients.All.SendAsync("LOCATION_UPDATE", new
            {
                vehicleId,
                lat,
                lng,
                timestamp
            });
        }

        public async Task SendLowFuelAlert(string vehicleId, double remainingHours)
        {
            await _hub.Clients.All.SendAsync("LOW_FUEL_ALERT", new
            {
                vehicleId,
                remainingHours
            });
        }
    }
}
