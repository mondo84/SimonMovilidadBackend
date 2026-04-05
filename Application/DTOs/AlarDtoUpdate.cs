using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class AlarDtoUpdate
    {
        [Required]
        [JsonPropertyName("Id")]
        public int Id {  get; set; }

        [Required]
        [JsonPropertyName("Status")]
        public int Status { get; set; }

        [Required]
        [JsonPropertyName("Note")]
        public string Note { get; set; } = string.Empty;
    }
}
