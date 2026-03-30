using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace IotUnitTest
{
   
    public class MaskTest
    {
        [Fact]
        public void Mascara_correcta()
        {
            var result = SensorService.MaskVehicleId("DEV-1234-XC54");

            Assert.Equal("DEV-****", result);
        }
    }
}
