namespace YTPlaylistSearcherWebApp.DTOs
{
    public class SearchChipBagDTO
    {
        public List<SearchChipDTO> Chips { get; set; }
        public bool OrderByDesc { get; set; }
    }

    public class SearchChipDTO
    {
        public string ChipType { get; set; }
        public string Value { get; set; }
        public string Modifier { get; set; }
    }
}
