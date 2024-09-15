using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Runtime.Versioning;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.Data.CS;
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

                baseQ = baseQ.Include(x => x.Playlistvideos)
                                .ThenInclude(x => x.Video);

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

        public async Task<IEnumerable<Video>> SearchVideos(YTPSContext context, AdvancedSearchRequestDTO searchRequest)
        {
            if (string.IsNullOrWhiteSpace(searchRequest.SearchPhrase))
            {
                // make a query that start at a random index and grab 25 videos, run it 4 times then generate guid and sort the 100 videos
                var totalVideos = new List<Video>();

                // chop the max result size up
                for (int i = 0; i < 4; i++)
                {
                    int totalRecords = context.Videos.Count();
                    int skip = new Random().Next(0, totalRecords - 25);
                    totalVideos.AddRange(await context.Videos.Skip(skip)
                        .Take(25)
                        .ToListAsync());
                }

                return totalVideos.OrderBy(x => Guid.NewGuid());
            }

            // This query searches the videos table where the title of the video or the channel title who uploaded the video
            // contains our search input. We include the playlist videos so we can display which playlists the song already belongs to in the DB.
            return await context.Videos.Where(x =>
                x.Title.ToLower().Contains(searchRequest.SearchPhrase.ToLower()) ||
                x.ChannelTitle.ToLower().Contains(searchRequest.SearchPhrase.ToLower())
                )
                .Include(x => x.Playlistvideos)
                .ThenInclude(x => x.Playlist)
                .ToListAsync();
        }

        public async Task<IEnumerable<Videocomment>> GetVideoComments(YTPSContext context, int videoID)
        {
            return await context.Videocomments.Include(x => x.User).Where(x => x.VideoId == videoID).ToListAsync();
        }

        public async Task AddComment(YTPSContext context, Videocomment entity)
        {
            entity.CreatedDate = DateTime.Now;
            context.Videocomments.Add(entity);
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
        Task<IEnumerable<Video>> SearchVideos(YTPSContext context, AdvancedSearchRequestDTO searchRequest);
        Task<IEnumerable<Videocomment>> GetVideoComments(YTPSContext context, int videoID);
        Task AddComment(YTPSContext context, Videocomment entity);
    }
}
