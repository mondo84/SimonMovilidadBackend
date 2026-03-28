using Application.DTOs;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAlertService
    {
        Task<double> FuelPrediction(SensorDto dto);
    }
}
