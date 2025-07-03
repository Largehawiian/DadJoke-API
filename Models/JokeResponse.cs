using System.Text.Json.Serialization;

namespace DadJoke_API.Models
{
    public class JokeResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("joke")]
        public string Joke { get; set; }

        [JsonPropertyName("setup")]
        public string Setup { get; set; }

        [JsonPropertyName("delivery")]
        public string Delivery { get; set; }

        public override string ToString()
        {
            return Type switch
            {
                "single" => Joke,
                "twopart" => $"{Setup}{Environment.NewLine}{Delivery}",
                _ => Joke
            };
        }
    }
}
