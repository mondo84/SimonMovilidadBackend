using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using IotApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IotApp.RealTime
{
    public class SignalrNotifier(IHubContext<AlertsHub> hub) : IAlertHub
    {
        private readonly IHubContext<AlertsHub> _hub = hub;

        public async Task SendLocationUpdate(SensorData position)
        {
            await _hub.Clients.All.SendAsync("LOCATION_UPDATE_ONE", position);
        }

        public async Task SendLocationUpdateList(List<SensorData> historyList)
        {
            await _hub.Clients.All.SendAsync("LOCATION_UPDATE", historyList);
        }

        public async Task SendLowFuelAlert(AlarmDto dto)
        {
            await _hub.Clients.All.SendAsync("LOW_FUEL_ALERT", dto);
        }
    }
}
