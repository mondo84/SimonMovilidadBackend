using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class SensorDataDto
    {
        [Required]
        [JsonPropertyName("Vehicle_id")]
        public string VehicleId { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("Lat")]
        public double Lat { get; set; }

        [Required]
        [JsonPropertyName("Long")]
        public double Long { get; set; }

        [Required]
        [JsonPropertyName("FuelLevel")]
        public double FuelLevel { get; set; }

        [Required]
        [JsonPropertyName("Temperature")]
        public double Temperature { get; set; }

        [Required]
        [JsonPropertyName("Speed")]
        public float Speed { get; set; }

        [Required]
        [JsonPropertyName("Timestamp")]
        public DateTime Timestamp { get; set; }

        [Required]
        [JsonPropertyName("Active")]
        public bool Active { get; set; } = true;
    }
}
