using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using BeProductive.Modules.Users.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BeProductive.Modules.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvatarController : Controller {
    private readonly UserAvatarService _userAvatarService;

    public AvatarController(UserAvatarService userAvatarService)
    {
        _userAvatarService = userAvatarService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvatar()
    {
        var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        Log.Warning("User id: {userId}", userId);
        
        var stream = await _userAvatarService.GetUserAvatar(userId);
        Log.Warning("Stream: {stream}", stream);
        if (stream is not null)
        {
            return File(stream, "image/png");
        }
        
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        var userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await using var stream = file.OpenReadStream();

        var status = await _userAvatarService.UpdateAvatar(userId, stream);

        if (status)
        {
            return Ok();
        }

        return BadRequest();
    }
}