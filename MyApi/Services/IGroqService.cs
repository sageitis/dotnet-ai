namespace MyApi.Services;
public interface IGroqService
{
    Task<string> GetChatResponseAsync(string prompt);
}