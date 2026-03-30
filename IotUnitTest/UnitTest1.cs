//namespace IotUnitTest
//{
//    public class UnitTest1
//    {

//    }
//}
using Xunit;
using Moq;
using Application.Services;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace IotUnitTest
{
    public class AlertServiceTest
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<ISensorRepository> _repo = new();
        private readonly AlertService _service;

        public AlertServiceTest()
        {
            _uow.Setup(x => x.Sensors).Returns(_repo.Object);
            _service = new AlertService(_uow.Object);
        }

        [Fact]
        public async Task Retorna_infinito_si_no_hay_datos()
        {
            var dto = new SensorDto { VehicleId = "DEV-1234567-XY" };

            _repo.Setup(x => x.GetSensorDataListAsync("DEV-1234567-XY"))
                .ReturnsAsync(new List<SensorData>());

            var result = await _service.FuelPrediction(dto);

            Assert.Equal(double.PositiveInfinity, result);
        }


        [Fact]
        public async Task Calcula_prediccion_correcta()
        {
            var now = DateTime.UtcNow;

            var dto = new SensorDto { VehicleId = "DEV-1234567-XY" };

            var data = new List<SensorData>
            {
                new () { FuelLevel = 40, Timestamp = now },
                new () { FuelLevel = 50, Timestamp = now.AddHours(-1) }
            };

            _repo.Setup(x => x.GetSensorDataListAsync("1"))
                .ReturnsAsync(data);

            var result = await _service.FuelPrediction(dto);

            Assert.Equal(4, result, 1); // (combustible actual = 40) / (promedio combustible * hora = 10) = 4 horas.
        }
    }
}


