namespace YTPlaylistSearcherWebApp.Models
{
    public class SharedPostDTO
    {
        public int postID { get; set; }
        public string userName { get; set; }
        public string content { get; set; }
        public string thumbnail { get; set; }
        public string link { get; set; }
        public string type { get; set; }
        public string sharedDate { get; set; }
        public bool isOwned { get; set; }
    }
}
