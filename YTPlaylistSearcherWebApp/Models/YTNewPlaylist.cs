namespace YTPlaylistSearcherWebApp.Models
{
    // Root
    public class YTNewPlaylist
    {
        public YTNewPlaylistSnippet snippet { get; set; }
        public Status status { get; set; }
    }

    public class YTNewPlaylistSnippet
    {
        public string title { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; } = new List<string> { "YTPlaylistSearcher", };
        public string defaultLanguage { get; set; } = "en";
    }

    public class Status
    {
        public string privacyStatus { get; set; } = "Private";
    }
}
