namespace YTPlaylistSearcherWebApp.DTOs
{
    public class HoneyMediaDTO
    {
        public int Id { get; set; }
        public string MediaTitle { get; set; } = null!;
        public string? Pitch { get; set; }
        public int? MediaTypeId { get; set; }
        public string? MediaTypeName { get; set; }
        public string? MediaTypeColor { get; set; }
        public int? InterestTypeId { get; set; }
        public string? InterestTypeName { get; set; }
        public string? InterestTypeColor { get; set; }
        public int? RequestingUserId { get; set; }
        public string? RequestingUserName { get; set; }
        public string? RequestingUserColor { get; set; }
        public int? ResponseId { get; set; }
        public string? Response { get; set; }
        public string? ResponseColor { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? StatusColor { get; set; }
        public int? NoelleRating { get; set; }
        public int? AaronRating { get; set; }
        public string? NoelleComment { get; set; }
        public string? AaronComment { get; set; }
        public string? Tracker { get; set; }
        public DateTime? DateRequested { get; set; }
        public DateTime? DateFinished { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class MediaTypeDTO
    {
        public int Id { get; set; }
        public string? MediaTypeName { get; set; }
        public string? Color { get; set; }
    }

    public class MediaInterestDTO
    {
        public int Id { get; set; }
        public string? InterestTypeName { get; set; }
        public string? Color { get; set; }
    }

    public class MediaRequesterDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
    }

    public class MediaResponseDTO
    {
        public int Id { get; set; }
        public string? Response { get; set; }
        public string? Color { get; set; }
    }

    public class MediaStatusDTO
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Color { get; set; }
    }
}

