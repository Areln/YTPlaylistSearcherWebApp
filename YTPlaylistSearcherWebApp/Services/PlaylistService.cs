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

        public async Task<PlaylistDTO> GetPlaylistSorted(YTPSContext context, string playlistID, SearchChipBagDTO bag)
        {
            var dbPlaylist = await GetPlaylist(context, playlistID);
            var sorted = dbPlaylist.Videos;
            dbPlaylist.Videos = sorted.OrderByDescending(x => x.PublishedDate);
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

            var vidsToRemove = dbPlaylist.Videos
                .Where(x => ytModelPlaylist.Videos
                    .Where(z => z.VideoId == x.VideoId)
                    .Any() == false)
                .ToList();

            var newVids = ytModelPlaylist.Videos
                .Where(x => dbPlaylist.Videos
                    .Where(z => z.VideoId == x.VideoId)
                    .Any() == false)
                .ToList();

            // Update DB
            await _playlistRepository.DeleteVideos(context, vidsToRemove);
            dbPlaylist.Videos = dbPlaylist.Videos.Concat(newVids).ToList();

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
                Content = sharedPostModel.Type == "video" ? context.Videos.Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().Title : context.Playlists.Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().PlaylistTitle,
                CreatedDate = DateTime.Now,
                Thumbnail = sharedPostModel.Type == "video" ? context.Videos.Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().Thumbnail : thumbnails,
                Link = sharedPostModel.Type == "video" ? context.Videos.Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().VideoId : context.Playlists.Where(x => x.Id == sharedPostModel.ContentID).FirstOrDefault().PlaylistId,
                Type = sharedPostModel.Type,
            };

            await _playlistRepository.AddSharedPost(context, newPost);
            await context.SaveChangesAsync();
            await _shareFeedHub.Clients.All.SendAsync(WebSocketActions.NEW_POST, JsonConvert.SerializeObject(PlaylistMapper.MapToDTO(newPost)));
            
            return newPost.Id;
        }

        public async Task<IEnumerable<string>> GetPlaylistThumbnails(YTPSContext context, int playlistID)
        {
            var temp = context.Playlists.Include(x => x.Videos).Where(x => x.Id == playlistID).FirstOrDefault().Videos.Take(6).Select(x => x.Thumbnail).AsEnumerable();
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
    }
}
