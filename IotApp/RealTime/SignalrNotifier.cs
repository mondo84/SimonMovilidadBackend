using Application.Interfaces;
using Domain.Entities;
using IotApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IotApp.RealTime
{
    public class SignalrNotifier(IHubContext<AlertsHub> hub) : IAlertHub
    {
        private readonly IHubContext<AlertsHub> _hub = hub;

        public async Task SendLocationUpdate(List<SensorData> historyList)
        {
            await _hub.Clients.All.SendAsync("LOCATION_UPDATE", historyList);
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
