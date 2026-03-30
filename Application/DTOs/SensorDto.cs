
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class SensorDto
    {
        [Required]
        [JsonPropertyName("vehicleId")]
        public string VehicleId { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("latitude")]
        public double Lat { get; set; }

        [Required]
        [JsonPropertyName("longitude")]
        public double Long { get; set; }

        [Required]
        [JsonPropertyName("fuelLevel")]
        public double FuelLvl { get; set; }

        [Required]
        [JsonPropertyName("speed")]
        public float Speed { get; set; }

        [Required]
        [JsonPropertyName("temperature")]
        public double Temp { get; set; }

        [Required]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [Required]
        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }
}
