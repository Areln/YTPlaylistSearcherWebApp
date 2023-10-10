namespace YTPlaylistSearcherWebApp.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string? ProfilePicture { get; set; }
        public string Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
