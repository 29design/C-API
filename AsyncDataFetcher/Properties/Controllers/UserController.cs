using Microsoft.AspNetCore.Mvc;
using AsyncDataFetcher.Services;

namespace AsyncDataFetcher.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{
    private readonly JsonPlaceholderClient _client;

    public UsersController(JsonPlaceholderClient client)
    {
        _client = client;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _client.FetchUserDataAsync(id);
        if (result.User == null)
            return NotFound(new { error = "User not found" });

        return Ok(result);
    }
}