using System.Text.Json.Serialization;

namespace DadJoke_API.Models
{
    public class SimpleJokeResponse
    {
        [JsonPropertyName("joke")]
        public string Joke { get; set; }
    }
}
