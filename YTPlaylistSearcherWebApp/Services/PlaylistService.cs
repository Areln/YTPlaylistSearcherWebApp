using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;
using YTPlaylistSearcherWebApp.Repositories;

namespace YTPlaylistSearcherWebApp.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistService(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public async Task<PlaylistDTO> GetPlaylistFromYT(string playlistID)
        {
            var result = await _playlistRepository.GetPlaylistFromYT(playlistID);
            return new PlaylistDTO
            {
                PlaylistID = playlistID,
                Videos = result.items.Where(x => x.snippet.title.ToLower() != "private video" && x.snippet.title.ToLower() != "deleted video").Select(x => new VideoDTO
                {
                    VideoID = x.contentDetails.videoId,
                    Title = x.snippet.title,
                    ChannelTitle = x.snippet.videoOwnerChannelTitle,
                    Description = x.snippet.description,
                    Thumbnail = x.snippet.thumbnails?.high?.url,
                    PublishedDate = x.snippet.publishedAt
                }),
            };
        }
    }

    public interface IPlaylistService
    {
        Task<PlaylistDTO> GetPlaylistFromYT(string playlistID);
    }
}
