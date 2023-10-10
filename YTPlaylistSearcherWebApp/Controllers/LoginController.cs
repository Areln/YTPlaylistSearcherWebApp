using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;
using YTPlaylistSearcherWebApp.Services;

namespace YTPlaylistSearcherWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<PlaylistController> _logger;
        private readonly YTPSContext _context;
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;

        public LoginController(ILogger<PlaylistController> logger, YTPSContext context, ILoginService loginService, ITokenService tokenService)
        {
            _logger = logger;
            _context = context;
            _loginService = loginService;
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromBody] LoginModel loginData)
        {
            try
            {
                if (loginData is null)
                {
                    return BadRequest("Invalid client request");
                }

                var userDTO = await _loginService.AttemptLogin(_context, loginData);

                if (userDTO is null)
                    return Unauthorized();

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userDTO.UserName),
                        new Claim(ClaimTypes.Role, userDTO.Role)
                    };
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                userDTO.RefreshToken = refreshToken;
                userDTO.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                await _loginService.UpdateRefreshToken(_context, userDTO);

                return Ok(new AuthenticatedResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken
                });

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Submit");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] LoginModel loginData)
        {
            try
            {
                var userDTO = await _loginService.SubmitRegistration(_context, loginData);

                if (userDTO is null)
                    return Unauthorized();

                var claims = new List<Claim>
                {
                        new Claim(ClaimTypes.Name, userDTO.UserName),
                        new Claim(ClaimTypes.Role, userDTO.Role)
                    };
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                userDTO.RefreshToken = refreshToken;
                userDTO.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                await _loginService.UpdateRefreshToken(_context, userDTO);

                return Ok(new AuthenticatedResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }
    }
}
