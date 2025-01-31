using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using YTPlaylistSearcherWebApp.Data;
using Google.Apis.Auth;
using YTPlaylistSearcherWebApp.Models;
using Newtonsoft.Json.Linq;

namespace YTPlaylistSearcherWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleAuthentication : ControllerBase
    {
        private readonly ILogger<PlaylistController> _logger;
        private readonly YTPSContext _context;

        public GoogleAuthentication(ILogger<PlaylistController> logger, YTPSContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("callback")]
        public async Task<IActionResult> GoogleCallback([FromBody] GoogleTokenModel token)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(token.IdToken);
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(new { message = "Invalid Google token" });
            }

            // At this point, the Google token is valid, and you have access to the user information in `payload`.
            // Now you can create your own JWT based on this information.

            // Example: Find or create the user in your database using the email in `payload.Email`
            // var user = await _userService.FindOrCreateUserAsync(payload.Email, payload.Name);

            // Generate your own JWT token
            //var jwtToken = _jwtService.GenerateToken(user);

            return Ok();
        }
    }
}
