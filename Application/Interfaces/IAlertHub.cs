using Application.DTOs;
using Domain.Entities;
using System.Text.Json.Serialization;

namespace Application.Interfaces
{
    public interface IAlertHub
    {
        Task SendLocationUpdate(List<SensorData> dto);
        Task SendLowFuelAlert(AlarmDto dto);
    }
}
