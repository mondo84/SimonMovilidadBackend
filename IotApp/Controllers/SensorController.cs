using Application.DTOs;
using Application.Interfaces;
using Application.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IotApp.Controllers
{
    [Route("api/sensor")]
    [ApiController]
    public class SensorController(ISensorService service) : ControllerBase
    {
        private readonly ISensorService _sensorService = service;

        [Authorize(Roles = "Admin, User, Viewer")]
        [HttpPost("data")]
        public async Task<AppResponse<List<SensorData>>> CaptureSensorData(List<SensorDto> dtoList)
        {
            return await _sensorService.SaveData(dtoList);
        }

        [Authorize(Roles = "Admin, User, Viewer")]
        [HttpGet()]
        public Task<AppResponse<List<SensorDataDto>>> GetSensorDataList([FromQuery] DateOnly date, [FromQuery] bool showInactive = false)
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return _sensorService.GetSensorDataListAsync(date, showInactive, role);
        }

        [Authorize(Roles = "Admin, User, Viewer")]
        [HttpPost("create/alert")]
        public async Task<AppResponse<AlarmDto>> Create(AlarmDto dto)
        {
            return await _sensorService.CreateAlarmAsync(dto);
        }

        [Authorize(Roles = "Admin, User, Viewer")]
        [HttpPut("update/alert")]
        public async Task<AppResponse<Alerts>> Update(AlarDtoUpdate dto)
        {
            return await _sensorService.UpdateAlarmAsync(dto);
        }

        [Authorize(Roles = "Admin, User, Viewer")]
        [HttpPost("list/alerts")]
        public async Task<AppResponse<List<AlarmDto>>> GetAlertList(AlarmReqDto dto)
        {
            return await _sensorService.GetAlertListAsync(dto);
        }
    }
}
