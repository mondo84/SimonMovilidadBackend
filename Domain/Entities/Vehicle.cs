using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Vehicle : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Model")]
        [JsonPropertyName("Model")]
        public string Model { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("Make")]
        [JsonPropertyName("Make")]
        public string Make { get; set; } = string.Empty;

        [Required]
        [Column("Active")]
        [JsonPropertyName("Active")]
        public bool Active { get; set; } = true;

    }
}
