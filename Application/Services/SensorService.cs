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

        public async Task<AppResponse<List<SensorDataDto>>> GetSensorDataListAsync(DateOnly date, bool showInactive, string? role)
        {
            var resp = await _unitOfWork.Sensors.GetAllAsync(date, showInactive);

            var hasRole = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);

            var result = resp.Select(x => new SensorDataDto
            {
                Id = x.Id,
                VehicleId = hasRole ? x.VehicleId : MaskVehicleId(x.VehicleId),
                Lat = x.Lat,
                Long = x.Long,
                FuelLevel = x.FuelLevel,
                Temperature = x.Temperature,
                Speed = x.Speed,
                Timestamp = x.Timestamp,
                Active = x.Active,
            }).ToList();

            return AppResponse<List<SensorDataDto>>.Ok(result, "Historial sensor");
        }

        public async Task<AppResponse<List<SensorData>>> SaveData(List<SensorDto> listDto)
        {
            if (listDto == null || listDto.Count == 0)
                throw new AppException(HttpStatusCode.BadRequest, "Sensor data es requerido");

            await SaveDataBatch(listDto);

            var dto = listDto.Last()!;

            // Calcular autonomía
            var remainingHours = await _alertService.FuelPrediction(dto);

            // Si menos de 1 hora de autonomia, manda la alerta. Si han pasado mas de 20 minutos desde la ultima ubicacion.
            if (remainingHours < 1) 
            {
                var lastAlert = await _unitOfWork.Alerts
                    .GetLastAlertByVehicle(dto.VehicleId);

                if(lastAlert == null || lastAlert.CreatedAt < DateTime.UtcNow.AddMinutes(-20))
                {
                    var messageAlarm = "Alerta de combustible bajo";
                    var alarmDto = new AlarmDto()
                    {
                        VehicleId = dto.VehicleId,
                        Message = messageAlarm,
                        Lat = dto.Lat,
                        Long = dto.Long,
                        Active = dto.Active,
                        CreatedAt = DateTime.UtcNow,
                    };

                    await CreateAlarmAsync(alarmDto);

                    // Alerta combustible bajo. SignalR.
                    await _notifier.SendLowFuelAlert(alarmDto);
                }
            }
            var NumberOfRecentRecords = 20;
            var sensorHistory = await _unitOfWork.Sensors.TodaysSensorDataHistory(NumberOfRecentRecords);

            if (sensorHistory.Count > 0)
            {
                var latestSensor = sensorHistory.Last();
                await _notifier.SendLocationUpdate(latestSensor);   // Mobile. Map.
                await _notifier.SendLocationUpdateList(sensorHistory); // Web. History.
            }

            return AppResponse<List<SensorData>>.Ok(sensorHistory, "Datos guardados exitosamente");
        }

        public static string MaskVehicleId(string? vehicleId)
        {
            var textMask = "DEV-****";
            if (string.IsNullOrWhiteSpace(vehicleId)) return textMask;

            return textMask;
        }

        public async Task<AppResponse<AlarmDto>> CreateAlarmAsync(AlarmDto dto)
        {
            var Alarm = new Alerts()
            {
                VehicleId=dto.VehicleId,
                Message = dto.Message,
                Lat = dto.Lat,
                Long = dto.Long,
                Active = dto.Active,
                CreatedAt = DateTime.Now,
            };

            await _unitOfWork.Alerts.AddAsync(Alarm);
            await _unitOfWork.SaveChangesAsync();

            var result = new AlarmDto
            {
                Id = Alarm.Id,
                VehicleId = Alarm.VehicleId,
                Message = Alarm.Message,
                Lat = Alarm.Lat,
                Long = Alarm.Long,
                Active = Alarm.Active,
                CreatedAt = Alarm.CreatedAt,
            };

            return AppResponse<AlarmDto>.Ok(result, "Alarma creada exitosamente");
        }

        public async Task<AppResponse<List<AlarmDto>>> GetAlertListAsync(AlarmReqDto dto)
        {
            var vehicleId = dto.VehicleId;
            var date = dto.Date;
            var showInactive = dto.ShowInactive;

            var respEntity = await _unitOfWork.Alerts.GetAlertListAsync(vehicleId, date, showInactive);
            var responseDto = respEntity
                .Select(s => new AlarmDto
                {
                    Id= s.Id,
                    VehicleId = s.VehicleId,
                    Message = s.Message,
                    Lat = s.Lat,
                    Long = s.Long,
                    Active = s.Active,
                    CreatedAt = s.CreatedAt
                })
                .ToList();

            return AppResponse<List<AlarmDto>>.Ok(responseDto, "Listado de alarmas");
        }

        private async Task SaveDataBatch(List<SensorDto> listDto)
        {
            if (listDto == null || listDto.Count == 0)
                return;

            foreach (var dto in listDto)
            {
                var sensorData = new SensorData()
                {
                    Lat = dto.Lat,
                    Long = dto.Long,
                    FuelLevel = dto.FuelLvl,
                    Speed = dto.Speed,
                    Temperature = dto.Temp,
                    VehicleId = dto.VehicleId,
                    Timestamp = dto.Timestamp
                };

                await _unitOfWork.Sensors.AddAsync(sensorData);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
