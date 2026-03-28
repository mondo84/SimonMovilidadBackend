using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Users : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("User_id")]
        [JsonPropertyName("User_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("First_name")]
        [JsonPropertyName("First_name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("Last_name")]
        [JsonPropertyName("Last_name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("Username")]
        [JsonPropertyName("Username")]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("Password")]
        [JsonPropertyName("Password")]
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("Active")]
        [JsonPropertyName("Active")]
        public bool Active { get; set; } = true; // Valor por defecto en memoria, no en la migracion.

        [Required]
        [ForeignKey("Role")]
        [Column("Role_Id")]
        [JsonPropertyName("Role_Id")]
        public int RoleId { get; set; }

        [JsonPropertyName("Role")]
        public Role? Role { get; set; }
    }
}
