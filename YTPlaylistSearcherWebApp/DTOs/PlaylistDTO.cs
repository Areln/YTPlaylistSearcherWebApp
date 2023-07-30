namespace YTPlaylistSearcherWebApp.DTOs
{
    public class PlaylistDTO
    {
        public string PlaylistID { get; set; }
        public string PlaylistTitle { get; set; }
        public IEnumerable<VideoDTO> Videos { get; set; }
    }
}
