using DadJoke_API.Models;
public interface IJokeService
{
    Task<string?> GetJokeAsync(int uriIndex = 1);
}

public class JokeService : IJokeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public JokeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string?> GetJokeAsync(int uriIndex = 1)
    {
        string uri = uriIndex == 2
            ? "https://jokeapi-v2.p.rapidapi.com/joke/Any?format=json&blacklistFlags=nsfw%2Cracist"
            : "https://icanhazdadjoke.com/";

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        switch (uriIndex)
        {
            case 1:
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                break;
            case 2:
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "cbae5abcbemsh1e9ed66cd850f8ep1dae6fjsn7b6ddb248914");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "jokeapi-v2.p.rapidapi.com");
                break;
        }

        try
        {
            string responseText = await client.GetStringAsync(uri);

            if (uriIndex == 1)
            {
                var jokeResponse = System.Text.Json.JsonSerializer.Deserialize<SimpleJokeResponse>(responseText);
                return jokeResponse?.Joke;
            }
            else
            {
                var jokeResponse = System.Text.Json.JsonSerializer.Deserialize<JokeResponse>(responseText);
                return jokeResponse?.ToString();
            }
        }
        catch (HttpRequestException ex)
        {
            // Log or handle error
        }
        catch (System.Text.Json.JsonException ex)
        {
            // Log or handle error
        }

        return null;
    }
}