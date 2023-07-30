namespace YTPlaylistSearcherWebApp.DTOs
{
    public class VideoDTO
    {
        public string VideoID { get; set; }
        public string Title { get; set; }
        public string ChannelTitle { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
