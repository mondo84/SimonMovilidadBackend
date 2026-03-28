using System.Text.Json.Serialization;

namespace Application.Response
{
    public class ErrorList
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = [];
    }
}
