using Application.DTOs;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISensorService
    {
        Task<AppResponse<SensorDto>> SaveData(SensorDto dto);
    }
}
