using Application.Common.Exceptions;
using Application.DTOs;
using Application.Interfaces;
using Application.Response;
using Domain.Entities;
using Domain.Interfaces;
using System.Net;

namespace Application.Services
{
    public class SensorService(IUnitOfWork unitOfWork, IAlertService alertService, IAlertHub notifier) : ISensorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAlertService _alertService = alertService;
        private readonly IAlertHub _notifier = notifier;

        public async Task<AppResponse<SensorDto>> SaveData(SensorDto dto)
        {
            var sensorData = new SensorData()
            {
                Lat = dto.Lat,
                Long = dto.Long,
                FuelLevel = dto.FuelLvl,
                Temperature = dto.Temp,
                VehicleId = dto.VehicleId,
                Timestamp = dto.Timestamp,
            };

            await _unitOfWork.Sensors.AddAsync(sensorData);
            await _unitOfWork.SaveChangesAsync();

            // Calcular autonomía
            var remainingHours = await _alertService.FuelPrediction(dto);
            
            if (remainingHours < 1)
            {
                // Alerta combustible bajo. SignalR.
                await _notifier.SendLowFuelAlert(dto.VehicleId, remainingHours);
            }

            // Enviar localizacion tiempo real. SignalR.
            await _notifier.SendLocationUpdate(sensorData);

            return AppResponse<SensorDto>.Ok(dto, "Datos guardados exitosamente");
        }
    }
}
