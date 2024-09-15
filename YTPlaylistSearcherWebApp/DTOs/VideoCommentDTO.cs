using YTPlaylistSearcherWebApp.Models;

namespace YTPlaylistSearcherWebApp.DTOs
{
    public class VideoCommentDTO
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = null!;
        public string CreatedDate { get; set; }
        public string? ModifiedDate { get; set; }
        public sbyte MembersOnly { get; set; }

        public UserDTO User { get; set; } = null!;
        public bool CanDelete { get; set; } = false;
    }
}
