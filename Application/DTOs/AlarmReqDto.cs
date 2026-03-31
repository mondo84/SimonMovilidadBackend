using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class AlarmReqDto
    {
        [JsonPropertyName("vehicleId")]
        public string? VehicleId {  get; set; }

        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }

        [JsonPropertyName("showInactive")]
        public bool ShowInactive { get; set; }
    }
}
