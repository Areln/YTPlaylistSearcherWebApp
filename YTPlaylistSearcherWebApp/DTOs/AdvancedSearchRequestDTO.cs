namespace YTPlaylistSearcherWebApp.DTOs
{
    public class AdvancedSearchRequestDTO
    {
        public string SearchPhrase { get; set; }
        public bool OrderByDesc { get; set; }
        public IEnumerable<SearchChipDTO> SearchChips { get; set; }
        public int Page { get; set; }
        public int MaxResultCount { get; set; }
    }

    public class SearchChipBagDTO
    {
        public List<SearchChipDTO> Chips { get; set; }
    }

    public class SearchChipDTO
    {
        public string ChipType { get; set; }
        public string Value { get; set; }
        public string Modifier { get; set; }
    }
}
