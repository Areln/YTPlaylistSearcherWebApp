using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
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

        public PlaylistController(ILogger<PlaylistController> logger, YTPSContext context, IPlaylistService playlistService)
        {
            _logger = logger;
            _playlistService = playlistService;
            _context = context;
        }

        [HttpGet("GetPlaylistFromYT")]
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

        [HttpGet("GetPlaylist")]
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

        [HttpGet("RefreshPlaylist")]
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

        [HttpGet("GetPlaylists")]
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
    }
}
