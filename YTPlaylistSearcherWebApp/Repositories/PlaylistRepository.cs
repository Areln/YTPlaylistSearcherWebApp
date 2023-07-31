﻿using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.EntityFrameworkCore;
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
                    playlist.items = playlist.items.Concat(tempList.items);
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

        public async Task<Playlist> GetPlaylist(YTPSContext context, string playlistID)
        {
            return await context.Playlists.Where(x => x.PlaylistId == playlistID)
                .Include(x => x.Videos)
                .FirstOrDefaultAsync();
        }

        public async Task AddPlaylist(YTPSContext context, Playlist newPlaylist)
        {
            await context.Playlists.AddAsync(newPlaylist);
        }

        async Task<string> MakeGetRequest(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }

    public interface IPlaylistRepository
    {
        Task<YTGetPlaylistDetailsResponse> GetPlaylistDetailsFromYT(string playlistID);
        Task AddPlaylist(YTPSContext context, Playlist newPlaylist);
        Task<Playlist> GetPlaylist(YTPSContext context, string playlistID);
        Task<YTPlaylist> GetPlaylistFromYT(string playlistID);
    }
}
