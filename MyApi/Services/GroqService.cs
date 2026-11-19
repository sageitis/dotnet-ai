using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace MyApi.Services;
public class GroqService : IGroqService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public GroqService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetChatResponseAsync(string prompt)
    {
        var apiKey = _config["Groq:GROQ_API_KEY"];
        var baseUrl = _config["Groq:BaseUrl"];
        var model = _config["Groq:Model"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            $"{baseUrl}/chat/completions",
            content
        );

        var responseString = await response.Content.ReadAsStringAsync();

        // Extract only useful text
        using var doc = JsonDocument.Parse(responseString);
        var result = doc
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
        
        return result ?? "No response";
    }
}