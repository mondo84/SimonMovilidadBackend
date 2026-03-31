using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class AlarmDto
    {
        [Required]
        [JsonPropertyName("Vehicle_id")]
        public string VehicleId { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("Lat")]
        public double Lat { get; set; }

        [Required]
        [JsonPropertyName("Long")]
        public double Long { get; set; }

        [Required]
        [JsonPropertyName("Active")]
        public bool Active { get; set; } = true;

        [JsonPropertyName("CreatedAt")]
        public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
