using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using YTPlaylistSearcherWebApp.Data;
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
                throw;
            }
        }

        [HttpGet("GetPlaylist")]
        public async Task<IActionResult> GetPlaylist([FromQuery] string playlistID)
        {
            try
            {
                var playlistDto = await _playlistService.GetPlaylist(_context, playlistID);
                return Ok(playlistDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetPlaylist");
                throw;
            }
        }


        //async void GetYoutubePlaylist(string playlistID = "PLNL_Z6NDFLJZafBMO4kA9PbNBq0t-f9hx", int maxResults = 50)
        //{
        //    //PLNL_Z6NDFLJZafBMO4kA9PbNBq0t-f9hx defualt id
        //    var response = JsonConvert.DeserializeObject<YTPlaylist>(await MakeRequest($"{ytHost}/playlistItems?part=snippet%2CcontentDetails&maxResults={maxResults}&playlistId={playlistID}&key={ytKey}", "GET"));

        //    if (response != null)
        //    {
        //        Playlist newPlaylist = new Playlist
        //        {
        //            createDate = DateTime.Now,
        //            title = DateTime.Now.ToString(),
        //            numberOfSongs = response.pageInfo.totalResults,
        //            songRecords = new List<SongRecord>()
        //        };

        //        var _playlistID = await AddOrUpdateNewPlaylist(newPlaylist);

        //        //get all playlist items into one list
        //        var _nextPageToken = response.nextPageToken;
        //        var totalPlaylistItems = response.items.ToList();

        //        while (totalPlaylistItems.Count < response.pageInfo.totalResults)
        //        {
        //            var tempResponse = JsonConvert.DeserializeObject<YTPlaylistItemContainer>(await MakeRequest($"{ytHost}/playlistItems?part=snippet%2CcontentDetails&maxResults={maxResults}&pageToken={_nextPageToken}&playlistId={playlistID}&key={ytKey}", "GET"));
        //            _nextPageToken = tempResponse.nextPageToken;
        //            totalPlaylistItems.AddRange(tempResponse.items);
        //        }

        //        //start downloading videos
        //        foreach (var item in totalPlaylistItems)
        //        {
        //            var _songRecord = await Control.Instance.DownloadVideo(item);
        //            newPlaylist.songRecords.Add(_songRecord);

        //            var temp = new PlaylistRecord
        //            {
        //                createDate = DateTime.Now,
        //                playlistID = _playlistID,
        //                songRecordID = _songRecord.recordID
        //            };
        //            await CreateNewPlaylistRecord(temp);
        //        }

        //        //await AddOrUpdateNewPlaylist(newPlaylist);
        //    }
        //}
    }
}
