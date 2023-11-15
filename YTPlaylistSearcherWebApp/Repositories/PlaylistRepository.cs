using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Runtime.Versioning;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;

namespace YTPlaylistSearcherWebApp.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        IConfiguration _configuration;
        IHttpClientFactory _httpClientFactory;

        const string YOUTUBE_HOST = "https://youtube.googleapis.com/youtube/v3";
        const int MAX_RESULTS = 100;

        public PlaylistRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        async Task<string> MakeGetRequest(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<YTPlaylist> GetPlaylistFromYT(string playlistID)
        {
            var client = _httpClientFactory.CreateClient();
            var ytKey = _configuration.GetValue(typeof(string), "YTKey");

            var response = await client.GetAsync($"{YOUTUBE_HOST}/playlistItems?part=snippet%2CcontentDetails&maxResults={MAX_RESULTS}&playlistId={playlistID}&key={ytKey}");

            var playlist = new YTPlaylist();

            if (response != null)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                playlist = JsonConvert.DeserializeObject<YTPlaylist>(json);

                var requestCount = 0;
                while (playlist.items.Count() < playlist.pageInfo.totalResults && requestCount < 40)
                {
                    response = await client.GetAsync($"{YOUTUBE_HOST}/playlistItems?part=snippet%2CcontentDetails&maxResults={MAX_RESULTS}&pageToken={playlist.nextPageToken}&playlistId={playlistID}&key={ytKey}");
                    var tempList = JsonConvert.DeserializeObject<YTPlaylist>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    
                    if (tempList?.items != null)
                    {
                        playlist.items = playlist.items.Concat(tempList.items);
                    }
                    else
                    {
                        break;
                    }

                    playlist.nextPageToken = tempList.nextPageToken;
                    requestCount++;
                }

            }

            return playlist;
        }

        public async Task<YTGetPlaylistDetailsResponse> GetPlaylistDetailsFromYT(string playlistID)
        {
            var ytKey = _configuration.GetValue(typeof(string), "YTKey");

            var jsonContent = await MakeGetRequest($"{YOUTUBE_HOST}/playlists?part=snippet&id={playlistID}&key={ytKey}");

            return JsonConvert.DeserializeObject<YTGetPlaylistDetailsResponse>(jsonContent);
        }

        public async Task<Playlist> GetPlaylist(YTPSContext context, string playlistID, IQueryable<Playlist>? playlistQuery = null, IQueryable<Video>? videoQuery = null)
        {
            try
            {
                var baseQ = context.Playlists
                    .Where(x => x.PlaylistId == playlistID);

                baseQ = baseQ.Include(x => x.Videos);

                if (playlistQuery != null)
                {
                    baseQ = baseQ.Concat(playlistQuery);
                }

                //var videoBaseQuery = context.Videos.AsQueryable();
                //if (videoQuery != null)
                //{
                //    videoBaseQuery = videoQuery;
                //}

                //baseQ = baseQ.Include(x => x.Videos.Concat(videoBaseQuery));

                var rr = await baseQ.FirstOrDefaultAsync();

                return rr;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        public async Task AddPlaylist(YTPSContext context, Playlist newPlaylist)
        {
            newPlaylist.UpdatedDate = DateTime.UtcNow;
            await context.Playlists.AddAsync(newPlaylist);
        }

        public async Task UpdatePlaylist(YTPSContext context, Playlist dbPlaylist)
        {
            dbPlaylist.UpdatedDate = DateTime.UtcNow;
            context.Playlists.Update(dbPlaylist);
        }

        public async Task DeleteVideos(YTPSContext context, Video video)
        {
            context.Videos.Remove(video);
        }

        public async Task DeleteVideos(YTPSContext context, IEnumerable<Video> video)
        {
            context.Videos.RemoveRange(video);
        }

        public async Task<IEnumerable<Playlist>> GetPlaylists(YTPSContext context)
        {
            return context.Playlists.AsEnumerable();
        }

        public async Task<IEnumerable<Sharedpost>> GetSharedPosts(YTPSContext context)
        {
            return context.Sharedposts.Include(x => x.User).OrderByDescending(x => x.CreatedDate).AsEnumerable();
        }

        public async Task AddSharedPost(YTPSContext context, Sharedpost newPost)
        {
            await context.Sharedposts.AddAsync(newPost);
        }

        public async Task<Sharedpost> GetPost(YTPSContext context, int id) 
        {
            return await context.Sharedposts.Include(x => x.User).FirstOrDefaultAsync(context => context.Id == id);
        }
    }

    public interface IPlaylistRepository
    {
        Task DeleteVideos(YTPSContext context, Video video);
        Task DeleteVideos(YTPSContext context, IEnumerable<Video> video);
        Task<YTGetPlaylistDetailsResponse> GetPlaylistDetailsFromYT(string playlistID);
        Task AddPlaylist(YTPSContext context, Playlist newPlaylist);
        Task<Playlist> GetPlaylist(YTPSContext context, string playlistID, IQueryable<Playlist>? playlistQuery = null, IQueryable<Video>? videoQuery = null);
        Task<YTPlaylist> GetPlaylistFromYT(string playlistID);
        Task UpdatePlaylist(YTPSContext context, Playlist dbPlaylist);
        Task<IEnumerable<Playlist>> GetPlaylists(YTPSContext _context);
        Task<IEnumerable<Sharedpost>> GetSharedPosts(YTPSContext context);
        Task AddSharedPost(YTPSContext context, Sharedpost newPost);
        Task<Sharedpost> GetPost(YTPSContext context, int id);
    }
}
