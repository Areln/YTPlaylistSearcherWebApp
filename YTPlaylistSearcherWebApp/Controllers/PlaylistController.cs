using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;
using YTPlaylistSearcherWebApp.Services;

namespace YTPlaylistSearcherWebApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly ILogger<PlaylistController> _logger;
        private readonly YTPSContext _context;
        private readonly IPlaylistService _playlistService;
        private readonly IHubContext<ShareFeedHub> _shareFeedHub;

        public PlaylistController(ILogger<PlaylistController> logger, YTPSContext context, IPlaylistService playlistService, IHubContext<ShareFeedHub> shareFeedHub)
        {
            _logger = logger;
            _playlistService = playlistService;
            _context = context;
            _shareFeedHub = shareFeedHub;
        }

        [HttpGet("GetPlaylistFromYT"), Authorize(Roles = "Admin, Trusted, Standard, Guest")]
        public async Task<IActionResult> GetPlaylistFromYT([FromQuery] string playlistID)
        {
            try
            {
                var playlist = await _playlistService.GetPlaylistFromYT(playlistID);
                return Ok(playlist);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetPlaylistFromYT");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }

        [HttpGet("GetPlaylist"), Authorize(Roles = "Admin, Trusted, Standard, Guest")]
        public async Task<IActionResult> GetPlaylist([FromQuery] string playlistID)
        {
            try
            {
                var searchBag = new SearchChipBagDTO();
                searchBag.Chips = new List<SearchChipDTO>();
                searchBag.Chips.Add(new SearchChipDTO
                {
                    ChipType = "OrderVideoBy",
                    Value = "PublishedDate",
                    Modifier = "false"
                });

                var playlistDto = await _playlistService.GetPlaylistSorted(_context, playlistID, searchBag);
                return Ok(playlistDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetPlaylist");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }

        [HttpGet("RefreshPlaylist"), Authorize(Roles = "Admin, Trusted, Standard, Guest")]
        public async Task<IActionResult> RefreshPlaylist([FromQuery] string playlistID)
        {
            try
            {
                var playlistDto = await _playlistService.RefreshPlaylist(_context, playlistID);
                return Ok(playlistDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetPlaylist");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }

        [HttpGet("GetPlaylists"), Authorize(Roles = "Admin, Trusted, Standard, Guest")]
        public async Task<IActionResult> GetPlaylists()
        {
            try
            {
                var playlistDto = await _playlistService.GetPlaylists(_context);
                return Ok(playlistDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetPlaylists");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }

        [HttpGet("GetSharedPosts"), Authorize(Roles = "Admin, Trusted, Standard, Guest")]
        public async Task<IActionResult> GetSharedPosts()
        {
            try
            {
                var username = User.Identity.Name;
                var playlistDto = await _playlistService.GetSharedPosts(_context, username);
                return Ok(playlistDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetSharedPosts");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }

        [HttpPost("CreateSharedPost"), Authorize(Roles = "Admin, Trusted, Standard")]
        public async Task<IActionResult> CreateSharedPost([FromBody] CreateSharedPostModel sharedPostModel)
        {
            try
            {
                sharedPostModel.UserName = User.FindFirst(ClaimTypes.Name).Value;
                var postID = await _playlistService.CreateSharedPost(_context, sharedPostModel, _shareFeedHub);
                return Ok(postID);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetSharedPosts");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }

        [HttpPost("DeletePost"), Authorize(Roles = "Admin, Trusted, Standard")]
        public async Task<IActionResult> DeletePost([FromBody] int id)
        {
            try
            {
                var username = User.Identity.Name;
                var wasDeleted = await _playlistService.DeletePost(_context, _shareFeedHub, id, username);
                return Ok(wasDeleted);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetSharedPosts");
                return BadRequest(e.Message + " " + e.InnerException);
            }
        }
    }
}
