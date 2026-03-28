using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class UserDto
    {
        [Required]
        [JsonPropertyName("User_id")]
        public int? UserId { get; set; }

        [Required]
        [JsonPropertyName("First_name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("Last_name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("Username")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("Active")]
        public bool? Active { get; set; }

        [JsonPropertyName("ChangePass")]
        public bool? ChangePass { get; set; } = false;

        [JsonPropertyName("NewPassword")]
        public string? NewPassword { get; set; }

        [JsonPropertyName("ConfirmPassword")]
        public string? ConfirmPassword { get; set; }

        [JsonPropertyName("Password")]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonPropertyName("Role_Id")]
        public int RoleId { get; set; }
    }
}
