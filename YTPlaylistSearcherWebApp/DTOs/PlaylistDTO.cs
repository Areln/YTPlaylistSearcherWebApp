namespace YTPlaylistSearcherWebApp.DTOs
{
    public class PlaylistDTO
    {
        public int ID { get; set; }
        public string PlaylistID { get; set; }
        public string PlaylistTitle { get; set; }
        public string ChannelOwner { get; set; }
        public IEnumerable<VideoDTO> Videos { get; set; }
    }
}
