using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Role : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Role_Id")]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Description")]
        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

       
        [Required]
        [Column("Active")]
        [JsonPropertyName("Active")]
        public bool Active { get; set; } = true; // Valor por defecto en memoria, no en la migracion.

    }
}
