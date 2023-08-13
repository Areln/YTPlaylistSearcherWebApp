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
                PlaylistID = playlistModel.PlaylistId,
                PlaylistTitle = playlistModel.PlaylistTitle,
                ChannelOwner = playlistModel.ChannelTitle,
                Videos = playlistModel.Videos.Select(x => new VideoDTO
                {
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
                PlaylistID = x.PlaylistId,
                PlaylistTitle = x.PlaylistTitle,
                ChannelOwner = x.ChannelTitle,
                Videos = x.Videos.Select(x => new VideoDTO
                {
                    VideoID = x.VideoId,
                    Title = x.Title,
                    Description = x.Description,
                    ChannelTitle = x.ChannelTitle,
                    PublishedDate = x.PublishedDate,
                    Thumbnail = x.Thumbnail
                })
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
