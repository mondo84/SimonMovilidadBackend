using Application.Common.Exceptions;
using Application.DTOs;
using Application.Interfaces;
using Application.Response;
using Domain.Entities;
using Domain.Interfaces;
using System.Net;

namespace Application.Services
{
    public class AlertService(IUnitOfWork unitOfWork) : IAlertService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        
        public async Task<double> FuelPrediction(SensorDto dto)
        {
            // Obtengo datos del sensor Order by desc.
            var sensorData = await _unitOfWork.Sensors.GetSensorDataListAsync(dto.VehicleId);

            if (sensorData.Count < 2)
                return double.PositiveInfinity;

            double totalConsumptionRate = 0;
            int validSegments = 0;

            for (int i = 0; i < sensorData.Count - 1; i++)  // Evita desbordamiento de array.
            {
                var currentRegister = sensorData[i];   // Registro mas reciente
                var previousRegister = sensorData[i + 1];  // Resgistro anterior.

                double hoursElapsed = (currentRegister.Timestamp - previousRegister.Timestamp).TotalHours;  // Horas trancurridas.

                if (hoursElapsed <= 0) continue;

                double fuelConsumed = previousRegister.FuelLevel - currentRegister.FuelLevel;   // Combustible consumido.

                if (fuelConsumed <= 0) continue;    // deja pasar recargas y datos errados.

                double consumptionPerHour = fuelConsumed / hoursElapsed;    // Consumo por hora.

                if (consumptionPerHour <= 0) continue;

                totalConsumptionRate += consumptionPerHour; // Total de consumo por hora.
                validSegments++;    // Contador de ciclos completos.
            }

            if (validSegments == 0)
                return double.PositiveInfinity;

            double avgConsumptionPerHour = totalConsumptionRate / validSegments;    // Promedio de consumo por hora.

            var latestRegister = sensorData.First();    // Registro mas reciente.

            return latestRegister.FuelLevel / avgConsumptionPerHour;  // Prediccion final: Combustible actual / coonsumo de promedio por hora.
        }
    }
}
