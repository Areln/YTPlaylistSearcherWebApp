using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using YTPlaylistSearcherWebApp.DTOs;

namespace YTPlaylistSearcherWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TMDBController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TMDBController> _logger;

        public TMDBController(HttpClient httpClient, IConfiguration configuration, ILogger<TMDBController> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("search/movie")]
        public async Task<IActionResult> SearchMovies([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] bool includeAdult = false)
        {
            try
            {
                var bearerToken = _configuration["TMDB:BearerToken"];
                if (string.IsNullOrEmpty(bearerToken))
                {
                    return BadRequest("TMDB Bearer token not configured");
                }

                var url = $"https://api.themoviedb.org/3/search/movie?query={Uri.EscapeDataString(query)}&include_adult={includeAdult.ToString().ToLower()}&language=en-US&page={page}";
                
                _logger.LogInformation($"Searching movies with query: {query}");
                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"TMDB API error: {response.StatusCode}");
                    return StatusCode((int)response.StatusCode, "Error calling TMDB API");
                }

                var content = await response.Content.ReadAsStringAsync();
                var tmdbResponse = JsonSerializer.Deserialize<TMDBMovieSearchResponse>(content);

                var results = tmdbResponse?.Results?.Select(movie => new VideoSearchDTO
                {
                    Id = movie.Id.ToString(),
                    Name = movie.Title,
                    Thumbnail = !string.IsNullOrEmpty(movie.PosterPath) ? $"https://image.tmdb.org/t/p/w500{movie.PosterPath}" : "https://via.placeholder.com/500x750?text=No+Image",
                    ReleaseYear = DateTime.TryParse(movie.ReleaseDate, out var releaseDate) ? releaseDate.Year : 0,
                    ContentType = "movie",
                    Adult = movie.Adult,
                    Description = movie.Overview,
                    Genre = string.Join(", ", movie.GenreIds?.Select(g => GetGenreName(g, "movie")).Where(g => !string.IsNullOrEmpty(g)) ?? new string[0])
                }).ToList() ?? new List<VideoSearchDTO>();

                return Ok(new TMDBVideoSearchResponse
                {
                    Results = results,
                    TotalResults = tmdbResponse?.TotalResults ?? 0,
                    Page = tmdbResponse?.Page ?? 1,
                    TotalPages = tmdbResponse?.TotalPages ?? 1
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching movies");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search/tv")]
        public async Task<IActionResult> SearchTVShows([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] bool includeAdult = false)
        {
            try
            {
                var bearerToken = _configuration["TMDB:BearerToken"];
                if (string.IsNullOrEmpty(bearerToken))
                {
                    return BadRequest("TMDB Bearer token not configured");
                }

                var url = $"https://api.themoviedb.org/3/search/tv?query={Uri.EscapeDataString(query)}&include_adult={includeAdult.ToString().ToLower()}&language=en-US&page={page}";
                
                _logger.LogInformation($"Searching TV shows with query: {query}");
                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"TMDB API error: {response.StatusCode}");
                    return StatusCode((int)response.StatusCode, "Error calling TMDB API");
                }

                var content = await response.Content.ReadAsStringAsync();
                var tmdbResponse = JsonSerializer.Deserialize<TMDBTVSearchResponse>(content);

                var results = tmdbResponse?.Results?.Select(tv => new VideoSearchDTO
                {
                    Id = tv.Id.ToString(),
                    Name = tv.Name,
                    Thumbnail = !string.IsNullOrEmpty(tv.PosterPath) ? $"https://image.tmdb.org/t/p/w500{tv.PosterPath}" : "https://via.placeholder.com/500x750?text=No+Image",
                    ReleaseYear = DateTime.TryParse(tv.FirstAirDate, out var firstAirDate) ? firstAirDate.Year : 0,
                    ContentType = "tv",
                    Adult = tv.Adult,
                    Description = tv.Overview,
                    Genre = string.Join(", ", tv.GenreIds?.Select(g => GetGenreName(g, "tv")).Where(g => !string.IsNullOrEmpty(g)) ?? new string[0])
                }).ToList() ?? new List<VideoSearchDTO>();

                return Ok(new TMDBVideoSearchResponse
                {
                    Results = results,
                    TotalResults = tmdbResponse?.TotalResults ?? 0,
                    Page = tmdbResponse?.Page ?? 1,
                    TotalPages = tmdbResponse?.TotalPages ?? 1
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching TV shows");
                return StatusCode(500, "Internal server error");
            }
        }

        private string GetGenreName(int genreId, string contentType)
        {
            // Movie genres
            if (contentType == "movie")
            {
                return genreId switch
                {
                    28 => "Action",
                    12 => "Adventure",
                    16 => "Animation",
                    35 => "Comedy",
                    80 => "Crime",
                    99 => "Documentary",
                    18 => "Drama",
                    10751 => "Family",
                    14 => "Fantasy",
                    36 => "History",
                    27 => "Horror",
                    10402 => "Music",
                    9648 => "Mystery",
                    10749 => "Romance",
                    878 => "Science Fiction",
                    10770 => "TV Movie",
                    53 => "Thriller",
                    10752 => "War",
                    37 => "Western",
                    _ => ""
                };
            }
            // TV genres
            else if (contentType == "tv")
            {
                return genreId switch
                {
                    10759 => "Action & Adventure",
                    16 => "Animation",
                    35 => "Comedy",
                    80 => "Crime",
                    99 => "Documentary",
                    18 => "Drama",
                    10751 => "Family",
                    10762 => "Kids",
                    9648 => "Mystery",
                    10763 => "News",
                    10764 => "Reality",
                    10765 => "Sci-Fi & Fantasy",
                    10766 => "Soap",
                    10767 => "Talk",
                    10768 => "War & Politics",
                    37 => "Western",
                    _ => ""
                };
            }
            return "";
        }
    }
}