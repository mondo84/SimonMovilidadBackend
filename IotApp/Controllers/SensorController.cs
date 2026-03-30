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
        public async Task<AppResponse<List<SensorData>>> CaptureSensorData(SensorDto request)
        {
            return await _sensorService.SaveData(request);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet()]
        public Task<AppResponse<List<SensorData>>> GetSensorDataList([FromQuery] bool showInactive = false)
        {
            return _sensorService.GetSensorDataListAsync(showInactive);
        }
    }
}
