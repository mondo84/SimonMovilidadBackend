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

        public async Task<AppResponse<List<SensorData>>> GetSensorDataListAsync(bool showInactive)
        {
            var resp = await _unitOfWork.Sensors.GetAllAsync(showInactive);

            return AppResponse<List<SensorData>>.Ok(resp, "Historial sensor");
        }

        public async Task<AppResponse<List<SensorData>>> SaveData(SensorDto dto)
        {
            var sensorData = new SensorData()
            {
                Lat = dto.Lat,
                Long = dto.Long,
                FuelLevel = dto.FuelLvl,
                Speed = dto.Speed,
                Temperature = dto.Temp,
                VehicleId = dto.VehicleId,
                Timestamp = dto.Timestamp.ToUniversalTime(),
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
            var NumberOfRecentRecords = 50;
            var sensorHistory = await _unitOfWork.Sensors.TodaysSensorDataHistory(NumberOfRecentRecords);
            await _notifier.SendLocationUpdate(sensorHistory); // Enviar localizacion tiempo real. SignalR.

            return AppResponse<List<SensorData>>.Ok(sensorHistory, "Datos guardados exitosamente");
        }
    }
}
