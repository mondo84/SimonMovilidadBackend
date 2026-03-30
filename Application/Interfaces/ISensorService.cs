using Application.DTOs;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISensorService
    {
        Task<AppResponse<List<SensorData>>> SaveData(SensorDto dto);

        Task<AppResponse<List<SensorData>>> GetSensorDataListAsync(bool showInactive);
    }
}
