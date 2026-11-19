using Microsoft.AspNetCore.Mvc;
using MyApi.Services;
using MyApi.DTOs;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IGroqService _groqService;

    public ChatController(IGroqService groqService)
    {
        _groqService = groqService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] PromptRequest request)
    {
        if (string.IsNullOrEmpty(request.Prompt))
            return BadRequest("Prompt cannot be empty");

        var result = await _groqService.GetChatResponseAsync(request.Prompt);

        return Ok(new { response = result });
    }
}