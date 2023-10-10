using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.Services;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly YTPSContext _context;
    private readonly ITokenService _tokenService;

    public TokenController(YTPSContext userContext, ITokenService tokenService, ILoginService loginService)
    {
        this._context = userContext ?? throw new ArgumentNullException(nameof(userContext));
        this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(TokenApiModel tokenApiModel)
    {
        if (tokenApiModel is null)
            return BadRequest("Invalid client request");

        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; //this is mapped to the Name claim by default

        var userAuth = _context.Users.Include(x => x.AuthenticationNavigation).Where(x => x.UserName == username).Select(x => x.AuthenticationNavigation).FirstOrDefault();

        if (userAuth is null || userAuth.RefreshToken != refreshToken || userAuth.RefreshDate <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        userAuth.RefreshToken = newRefreshToken;
        _context.SaveChanges();

        return Ok(new AuthenticatedResponse()
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var username = User.Identity.Name;

        var userAuth = _context.Users.Include(x => x.AuthenticationNavigation).Where(x => x.UserName == username).Select(x => x.AuthenticationNavigation).FirstOrDefault();
        if (userAuth == null) return BadRequest();

        userAuth.RefreshToken = null;

        _context.SaveChanges();

        return NoContent();
    }
}

public class TokenApiModel
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}