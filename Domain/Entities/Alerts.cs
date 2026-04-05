using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Alerts : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [Column("Vehicle_id")]
        [JsonPropertyName("Vehicle_id")]
        public string VehicleId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("Message")]
        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;

        [MaxLength(200)]
        [Column("Note")]
        [JsonPropertyName("Note")]
        public string? Note { get; set; }

        [Required]
        [JsonPropertyName("Lat")]
        public double Lat { get; set; }

        [Required]
        [JsonPropertyName("Long")]
        public double Long { get; set; }

        [Required]
        [JsonPropertyName("Status")]
        public int Status { get; set; } = 1;

        [Required]
        [Column("Active")]
        [JsonPropertyName("Active")]
        public bool Active { get; set; } = true;

    }
}
