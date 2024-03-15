using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.Data.CS;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Mappers;
using YTPlaylistSearcherWebApp.Models;
using YTPlaylistSearcherWebApp.Repositories;
using static YTPlaylistSearcherWebApp.Services.ShareFeedHub;

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
                Videos = listResult.items.Where(x => x.snippet.title.ToLower() != "private video"
                                                  && x.snippet.title.ToLower() != "deleted video")
                                                  .Select(x => new VideoDTO
                                                  {
                                                      VideoID = x.contentDetails.videoId,
                                                      Title = x.snippet.title,
                                                      ChannelTitle = x.snippet.videoOwnerChannelTitle,
                                                      //Description = x.snippet.description,
                                                      Thumbnail = x.snippet.thumbnails?.high?.url,
                                                      AddedToPlaylistDate = x.snippet.publishedAt
                                                  }),
            };
        }

        public async Task<PlaylistDTO> GetPlaylist(YTPSContext context, string playlistID)
        {
            // Check if the playlist id exists in the DB
            var dbPlaylist = await _playlistRepository.GetPlaylist(context, playlistID);
            var returnPlaylist = new PlaylistDTO();

            // If no result
            if (dbPlaylist == null)
            {
                // Get the playlist from youtube (PlaylistDTO)
                returnPlaylist = await GetPlaylistFromYT(playlistID);
                // DTO to models. I dont remember if there is a reason for mapping YT models to DTOs first,
                // may want to change it to be YT models -> DB models or it doesnt matter
                dbPlaylist = PlaylistMapper.MapToModel(returnPlaylist);

                // we want to re-use existing video records so check for them here
                for (int i = 0; i < dbPlaylist.Playlistvideos.Count; i++)
                {
                    var existingVideo = await context.Videos.FirstOrDefaultAsync(x => x.VideoId == dbPlaylist.Playlistvideos.ToList()[i].Video.VideoId);

                    if (existingVideo != null)
                        dbPlaylist.Playlistvideos.ToList()[i].Video.Id = existingVideo.Id;
                }

                // Add playlist to DB
                await _playlistRepository.AddPlaylist(context, dbPlaylist);
                await context.SaveChangesAsync();
            }
            else // else check if we should refresh the playlist or just map to DTO and return.
            {
                // TODO: Make Playlist Refresh Rate a setting we get from the DB
                if (dbPlaylist.UpdatedDate.AddMinutes(1) < DateTime.UtcNow)
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

        public async Task<PlaylistDTO> GetPlaylistSorted(YTPSContext context, string playlistID, SearchChipBagDTO bag)
        {
            var dbPlaylist = await GetPlaylist(context, playlistID);
            var sorted = dbPlaylist.Videos;
            dbPlaylist.Videos = sorted.OrderByDescending(x => x.AddedToPlaylistDate);
            return dbPlaylist;
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
            var ytModelPlaylist = PlaylistMapper.MapToModel(ytPlaylist);

            var vidsToRemove = dbPlaylist.Playlistvideos
                .Where(x => ytModelPlaylist.Playlistvideos
                    .Where(z => z.Video.VideoId == x.Video.VideoId)
                    .Any() == false)
                .ToList();

            var newVids = ytModelPlaylist.Playlistvideos
                .Where(x => dbPlaylist.Playlistvideos
                    .Where(z => z.Video.VideoId == x.Video.VideoId)
                    .Any() == false)
                .ToList();

            // we want to re-use existing video records so check for them here
            for (var i = 0; i < newVids.Count; i++)
            {
                var existingVideoID = await context.Videos.FirstOrDefaultAsync(x => x.VideoId == newVids[i].Video.VideoId);

                if (existingVideoID != null)
                    newVids[i].Video = existingVideoID;
            }

            // set db playlist videos = db playlist videos where vids to remove does not contain
            dbPlaylist.Playlistvideos = dbPlaylist.Playlistvideos.Where(x => vidsToRemove.Contains(x) == false).ToList();

            foreach (var v in newVids)
            {
                dbPlaylist.Playlistvideos.Add(v);
            }

            // TODO: update needs to re-use existing video ids
            await _playlistRepository.UpdatePlaylist(context, dbPlaylist);
            await context.SaveChangesAsync();

            // return fresh copy of db
            var freshDto = await GetPlaylistSorted(context, playlistID, new SearchChipBagDTO());

            return freshDto;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetPlaylists(YTPSContext _context)
        {
            var result = await _playlistRepository.GetPlaylists(_context);
            return PlaylistMapper.MapToDTO(result);
        }

        public async Task<IEnumerable<SharedPostDTO>> GetSharedPosts(YTPSContext _context, string username)
        {
            var result = await _playlistRepository.GetSharedPosts(_context);

            var dto = PlaylistMapper.MapToDTO(result).ToList();

            foreach (var item in dto)
            {
                if (item.userName == username)
                {
                    item.isOwned = true;
                }
            }

            return dto;
        }

        public async Task<int> CreateSharedPost(YTPSContext context, CreateSharedPostModel sharedPostModel, IHubContext<ShareFeedHub> _shareFeedHub)
        {
            string thumbnails = string.Empty;

            if (sharedPostModel.Type == "playlist")
            {
                var _thumbnails = await GetPlaylistThumbnails(context, sharedPostModel.ContentID);
                thumbnails = string.Join(',', _thumbnails);
            }

            var newPost = new Sharedpost
            {
                User = context.Users.Where(x => x.UserName == sharedPostModel.UserName).FirstOrDefault(),
                Content = sharedPostModel.Type == "video" ? context.Playlistvideos.Select(x => x.Video).Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().Title : context.Playlists.Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().PlaylistTitle,
                CreatedDate = DateTime.Now,
                Thumbnail = sharedPostModel.Type == "video" ? context.Playlistvideos.Select(x => x.Video).Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().Thumbnail : thumbnails,
                Link = sharedPostModel.Type == "video" ? context.Playlistvideos.Select(x => x.Video).Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().VideoId : context.Playlists.Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().PlaylistId,
                Type = sharedPostModel.Type,
            };

            await _playlistRepository.AddSharedPost(context, newPost);
            await context.SaveChangesAsync();
            await _shareFeedHub.Clients.All.SendAsync(WebSocketActions.NEW_POST, JsonConvert.SerializeObject(PlaylistMapper.MapToDTO(newPost)));

            return newPost.Id;
        }

        public async Task<IEnumerable<string>> GetPlaylistThumbnails(YTPSContext context, int playlistID)
        {
            var temp = context.Playlists.Include(x => x.Playlistvideos)
                                            .ThenInclude(x => x.Video)
                                                .Where(x => x.Id == playlistID)
                                                .FirstOrDefault().Playlistvideos.Take(6).Select(x => x.Video.Thumbnail).AsEnumerable();
            return temp;
        }

        public async Task<bool> DeletePost(YTPSContext context, IHubContext<ShareFeedHub> _shareFeedHub, int id, string username)
        {
            var post = await _playlistRepository.GetPost(context, id);

            if (post.User.UserName == username)
            {
                context.Sharedposts.Remove(post);
                await context.SaveChangesAsync();

                await _shareFeedHub.Clients.All.SendAsync(WebSocketActions.DELETE_POST, id);

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<VideoDTO>> SearchVideos(YTPSContext context, AdvancedSearchRequestDTO searchRequest)
        {
            var searchResults = await _playlistRepository.SearchVideos(context, searchRequest);
            //var dtos = PlaylistMapper.MapToDTO(searchResults).ToList();
            VideoDTO duplicateVideo = null;
            List<VideoDTO> returnList = new List<VideoDTO>();



            return returnList.Take(100);
        }
    }

    public interface IPlaylistService
    {
        Task<PlaylistDetailsDTO> GetPlaylistDetails(string playlistID);
        Task<PlaylistDTO> GetPlaylistFromYT(string playlistID);
        Task<PlaylistDTO> GetPlaylist(YTPSContext context, string playlistID);
        Task<PlaylistDTO> GetPlaylistSorted(YTPSContext context, string playlistID, SearchChipBagDTO bag);
        Task<PlaylistDTO> RefreshPlaylist(YTPSContext context, string playlistID);
        Task<IEnumerable<PlaylistDTO>> GetPlaylists(YTPSContext _context);
        Task<IEnumerable<SharedPostDTO>> GetSharedPosts(YTPSContext _context, string username);
        Task<int> CreateSharedPost(YTPSContext context, CreateSharedPostModel sharedPostModel, IHubContext<ShareFeedHub> _shareFeedHub);
        Task<bool> DeletePost(YTPSContext context, IHubContext<ShareFeedHub> _shareFeedHub, int id, string username);
        Task<IEnumerable<VideoDTO>> SearchVideos(YTPSContext context, AdvancedSearchRequestDTO searchRequest);
    }
}
