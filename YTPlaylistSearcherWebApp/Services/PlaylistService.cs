using System.Linq;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Mappers;
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
            var listResult = await _playlistRepository.GetPlaylistFromYT(playlistID);
            var detailsResult = PlaylistMapper.MapToDTO(await _playlistRepository.GetPlaylistDetailsFromYT(playlistID));

            return new PlaylistDTO
            {
                PlaylistTitle = detailsResult.Title,
                PlaylistID = playlistID,
                ChannelOwner = detailsResult.ChannelTitle,
                Videos = listResult.items.Where(x => x.snippet.title.ToLower() != "private video" && x.snippet.title.ToLower() != "deleted video").Select(x => new VideoDTO
                {
                    VideoID = x.contentDetails.videoId,
                    Title = x.snippet.title,
                    ChannelTitle = x.snippet.videoOwnerChannelTitle,
                    //Description = x.snippet.description,
                    Thumbnail = x.snippet.thumbnails?.high?.url,
                    PublishedDate = x.snippet.publishedAt
                }),
            };
        }

        public async Task<PlaylistDTO> GetPlaylist(YTPSContext context, string playlistID)
        {
            var dbPlaylist = await _playlistRepository.GetPlaylist(context, playlistID);
            var returnPlaylist = new PlaylistDTO();

            if (dbPlaylist == null)
            {
                returnPlaylist = await GetPlaylistFromYT(playlistID);
                dbPlaylist = PlaylistMapper.MapToModel(returnPlaylist);
                await _playlistRepository.AddPlaylist(context, dbPlaylist);
                await context.SaveChangesAsync();
            }
            else
            {
                // TODO: Make Playlist Refresh Rate a setting we get from the DB
                if (dbPlaylist.UpdatedDate.AddMinutes(15) < DateTime.UtcNow)
                {
                    returnPlaylist = await RefreshPlaylist(context, playlistID);
                }
                else
                {
                    returnPlaylist = PlaylistMapper.MapToDTO(dbPlaylist);
                }
            }

            return returnPlaylist;
        }

        public async Task<PlaylistDetailsDTO> GetPlaylistDetails(string playlistID)
        {
            var details = await _playlistRepository.GetPlaylistDetailsFromYT(playlistID);

            return PlaylistMapper.MapToDTO(details);
        }

        public async Task<PlaylistDTO> RefreshPlaylist(YTPSContext context, string playlistID)
        {
            var dbPlaylist = await _playlistRepository.GetPlaylist(context, playlistID);

            if (dbPlaylist == null)
            {
                return null;
            }

            // get YTPlaylist
            var ytPlaylist = await GetPlaylistFromYT(playlistID);
            var dtoPlaylist = PlaylistMapper.MapToModel(ytPlaylist);

            var vidsToRemove = dbPlaylist.Videos
                .Where(x => dtoPlaylist.Videos
                    .Where(z => z.VideoId == x.VideoId)
                    .Any() == false)
                .ToList();

            var newVids = dtoPlaylist.Videos
                .Where(x => dbPlaylist.Videos
                    .Where(z => z.VideoId == x.VideoId)
                    .Any() == false)
                .ToList();

            //vidsToRemove.ForEach(x =>
            //{
            //    dbPlaylist.Videos.Remove(dbPlaylist.Videos.Where(a => a.VideoId == x.VideoId).First());
            //});
            await _playlistRepository.DeleteVideos(context, vidsToRemove);
            
            dbPlaylist.Videos = dbPlaylist.Videos.Concat(newVids).ToList();

            await _playlistRepository.UpdatePlaylist(context, dbPlaylist);
            await context.SaveChangesAsync();

            return ytPlaylist;
        }
    }

    public interface IPlaylistService
    {
        Task<PlaylistDetailsDTO> GetPlaylistDetails(string playlistID);
        Task<PlaylistDTO> GetPlaylistFromYT(string playlistID);
        Task<PlaylistDTO> GetPlaylist(YTPSContext context, string playlistID);
        Task<PlaylistDTO> RefreshPlaylist(YTPSContext context, string playlistID);
    }
}
