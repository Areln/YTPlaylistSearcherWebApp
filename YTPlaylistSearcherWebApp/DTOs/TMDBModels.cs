using System.Text.Json.Serialization;

namespace YTPlaylistSearcherWebApp.DTOs
{
    public class TMDBMovieSearchResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }
        
        [JsonPropertyName("results")]
        public List<TMDBMovie> Results { get; set; } = new();
        
        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
        
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }

    public class TMDBMovie
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }
        
        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }
        
        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; } = new();
        
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; } = string.Empty;
        
        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; } = string.Empty;
        
        [JsonPropertyName("overview")]
        public string Overview { get; set; } = string.Empty;
        
        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }
        
        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
        
        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        
        [JsonPropertyName("video")]
        public bool Video { get; set; }
        
        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }
        
        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }
    }

    public class TMDBTVSearchResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }
        
        [JsonPropertyName("results")]
        public List<TMDBTVShow> Results { get; set; } = new();
        
        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
        
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }

    public class TMDBTVShow
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }
        
        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }
        
        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; } = new();
        
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("origin_country")]
        public List<string> OriginCountry { get; set; } = new();
        
        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; } = string.Empty;
        
        [JsonPropertyName("original_name")]
        public string OriginalName { get; set; } = string.Empty;
        
        [JsonPropertyName("overview")]
        public string Overview { get; set; } = string.Empty;
        
        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }
        
        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
        
        [JsonPropertyName("first_air_date")]
        public string? FirstAirDate { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }
        
        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }
    }

    public class TMDBVideoSearchResponse
    {
        public List<VideoSearchDTO> Results { get; set; } = new();
        public int TotalResults { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }

    public class VideoSearchDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string ContentType { get; set; } = string.Empty; // "movie" or "tv"
        public bool Adult { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public string? Genre { get; set; }
    }
}