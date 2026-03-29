using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class SensorData : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Vehicle_id")]
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
        [JsonPropertyName("Timestamp")]
        public DateTime Timestamp { get; set; }

        [Required]
        [JsonPropertyName("Active")]
        public bool Active { get; set; } = true;
    }
}
