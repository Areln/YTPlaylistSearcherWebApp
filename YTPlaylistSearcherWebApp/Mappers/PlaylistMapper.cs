using System.Net.NetworkInformation;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;

namespace YTPlaylistSearcherWebApp.Mappers
{
    public static class PlaylistMapper
    {

        // YT MODELS TO DB MODELS

        // DB MODELS TO DTO
        public static PlaylistDTO MapToDTO(Playlist playlistModel)
        {
            return new PlaylistDTO
            {
                ID = playlistModel.Id,
                PlaylistID = playlistModel.PlaylistId,
                PlaylistTitle = playlistModel.PlaylistTitle,
                ChannelOwner = playlistModel.ChannelTitle,
                Videos = playlistModel.Videos.Select(x => new VideoDTO
                {
                    ID = x.Id,
                    VideoID = x.VideoId,
                    Title = x.Title,
                    Description = x.Description,
                    ChannelTitle = x.ChannelTitle,
                    PublishedDate = x.PublishedDate,
                    Thumbnail = x.Thumbnail
                })
            };
        }

        public static IEnumerable<PlaylistDTO> MapToDTO(IEnumerable<Playlist> list)
        {
            return list.Select(x => new PlaylistDTO
            {
                ID = x.Id,
                PlaylistID = x.PlaylistId,
                PlaylistTitle = x.PlaylistTitle,
                ChannelOwner = x.ChannelTitle,
                Videos = x.Videos.Select(y => new VideoDTO
                {
                    ID = y.Id,
                    VideoID = y.VideoId,
                    Title = y.Title,
                    Description = y.Description,
                    ChannelTitle = y.ChannelTitle,
                    PublishedDate = y.PublishedDate,
                    Thumbnail = y.Thumbnail
                })
            });
        }

        public static IEnumerable<SharedPostDTO> MapToDTO(IEnumerable<Sharedpost> list)
        {
            return list.Select(x => new SharedPostDTO
            {
                UserName = x.User.UserName,
                Content = x.Content,
                Link = x.Link,
                SharedDate = x.CreatedDate.ToString(),
                Thumbnail = x.Thumbnail,
                Type = x.Type,
            });
        }

        // DTO TO DB MODELS
        public static Playlist MapToModel(PlaylistDTO playlistDTO)
        {
            return new Playlist
            {
                PlaylistId = playlistDTO.PlaylistID,
                PlaylistTitle = playlistDTO.PlaylistTitle,
                ChannelTitle = playlistDTO.ChannelOwner,
                Videos = playlistDTO.Videos.Select(x => new Video
                {
                    VideoId = x.VideoID,
                    Title = x.Title,
                    Description = x.Description,
                    ChannelTitle = x.ChannelTitle,
                    PublishedDate = x.PublishedDate,
                    Thumbnail = x.Thumbnail
                }).ToList(),
            };
        }

        // TY TO DTO
        public static PlaylistDetailsDTO MapToDTO(YTGetPlaylistDetailsResponse response)
        {
            return new PlaylistDetailsDTO
            {
                Title = response.items.First().snippet.title,
                Thumbnail = response.items.First().snippet.thumbnails.medium?.url,
                ChannelTitle = response.items.First().snippet.channelTitle,
                PlaylistID = response.items.First().id
            };
        }

    }
}
