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
                Videos = playlistModel.Playlistvideos.Select(x => new VideoDTO
                {
                    ID = x.Video.Id,
                    VideoID = x.Video.VideoId,
                    Title = x.Video.Title,
                    Description = x.Video.Description,
                    ChannelTitle = x.Video.ChannelTitle,
                    AddedToPlaylistDate = x.AddedDate,
                    Thumbnail = x.Video.Thumbnail
                })
            };
        }
        public static VideoDTO MapToDTO(Video video)
        {
            return new VideoDTO
            {
                ID = video.Id,
                VideoID = video.VideoId,
                Title = video.Title,
                Description = video.Description,
                ChannelTitle = video.ChannelTitle,
                //PublishedDate = video.PublishedDate,
                Thumbnail = video.Thumbnail,
                Playlists = new List<PlaylistDTO>()
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
                Videos = x.Playlistvideos.Select(y => new VideoDTO
                {
                    ID = y.Video.Id,
                    VideoID = y.Video.VideoId,
                    Title = y.Video.Title,
                    Description = y.Video.Description,
                    ChannelTitle = y.Video.ChannelTitle,
                    AddedToPlaylistDate = y.Video.PublishedDate,
                    Thumbnail = y.Video.Thumbnail
                })
            });
        }

        public static IEnumerable<SharedPostDTO> MapToDTO(IEnumerable<Sharedpost> list)
        {
            return list.Select(x => new SharedPostDTO
            {
                postID = x.Id,
                userName = x.User.UserName,
                content = x.Content,
                link = x.Link,
                sharedDate = x.CreatedDate.ToString(),
                thumbnail = x.Thumbnail,
                type = x.Type,
                isOwned = false
            });
        }

        public static IEnumerable<VideoDTO> MapToDTO(IEnumerable<Video> videos)
        {
            return videos.Select(y => new VideoDTO
            {
                ID = y.Id,
                VideoID = y.VideoId,
                Title = y.Title,
                Description = y.Description,
                ChannelTitle = y.ChannelTitle,
                //AddedToPlaylistDate = y.PublishedDate,
                Thumbnail = y.Thumbnail,
                Playlists = new List<PlaylistDTO>()
            });
        }

        public static SharedPostDTO MapToDTO(Sharedpost post)
        {
            return new SharedPostDTO
            {
                postID = post.Id,
                userName = post.User.UserName,
                content = post.Content,
                link = post.Link,
                sharedDate = post.CreatedDate.ToString(),
                thumbnail = post.Thumbnail,
                type = post.Type,
                isOwned = false
            };
        }

        // DTO TO DB MODELS
        public static Playlist MapToModel(PlaylistDTO playlistDTO)
        {
            return new Playlist
            {
                PlaylistId = playlistDTO.PlaylistID,
                PlaylistTitle = playlistDTO.PlaylistTitle,
                ChannelTitle = playlistDTO.ChannelOwner,
                Playlistvideos = playlistDTO.Videos.Select(x => new Playlistvideo 
                {
                    Video = new Video 
                    {
                        VideoId = x.VideoID,
                        Title = x.Title,
                        Description = x.Description,
                        ChannelTitle = x.ChannelTitle,
                        //PublishedDate = x.PublishedDate, 
                        // This is commented out because the publish date is actually the
                        // date it was added to the playlist and we only need to know that on a PlaylistVideo record
                        Thumbnail = x.Thumbnail
                    },
                    AddedDate = x.AddedToPlaylistDate
                }).ToList(),
            };
        }

        // YT TO DTO
        public static PlaylistDetailsDTO MapToDTO(YTGetPlaylistDetailsResponse response)
        {
            return new PlaylistDetailsDTO
            {
                Title = response.items.First().snippet.title,
                Thumbnail = response.items.First().snippet.thumbnails.high.url,
                ChannelTitle = response.items.First().snippet.channelTitle,
                PlaylistID = response.items.First().id
            };
        }

    }
}
