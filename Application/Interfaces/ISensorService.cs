using Application.DTOs;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISensorService
    {
        Task<AppResponse<List<SensorData>>> SaveData(List<SensorDto> listDto);

        Task<AppResponse<List<SensorDataDto>>> GetSensorDataListAsync(DateOnly date, bool showInactive, string? role);

        Task<AppResponse<AlarmDto>> CreateAlarmAsync(AlarmDto dto);

        Task<AppResponse<List<AlarmDto>>> GetAlertListAsync(AlarmReqDto dto);
    }
}
