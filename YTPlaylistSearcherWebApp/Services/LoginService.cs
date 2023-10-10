using System.Security.Cryptography;
using System.Text;
using YTPlaylistSearcherWebApp.Controllers;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;
using YTPlaylistSearcherWebApp.Repositories;

namespace YTPlaylistSearcherWebApp.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        private string HashPassword(string password, string salt)
        {
            // Combine the password and salt
            string saltedPassword = password + salt;

            // Create a SHA-256 hash algorithm instance
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the salted password
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                // Convert the byte array to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        private string GenerateSalt(int length)
        {
            // Generate a random salt of the specified length
            return BitConverter.ToString(RandomNumberGenerator.GetBytes(length)).Replace("-", "").ToLower();
        }

        public async Task<UserDTO> AttemptLogin(YTPSContext context, LoginModel loginData)
        {
            var user = await _loginRepository.GetUserByUserName(context, loginData.UserName);

            if (user == null)
            {
                return null;
            }

            //password check
            if (user.AuthenticationNavigation.Password == HashPassword(loginData.Password, user.AuthenticationNavigation.PasswordSalt))
            {
                // TODO: set last login??
                var userDto = new UserDTO
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    ProfilePicture = user.ProfilePicture,
                    Role = user.RoleNavigation.RoleName,
                };

                return userDto;
            }

            return null;
        }

        public async Task UpdateRefreshToken(YTPSContext context, UserDTO userDTO)
        {
            await _loginRepository.UpdateRefreshToken(context, userDTO);
            await context.SaveChangesAsync();
        }

        public async Task<UserDTO> SubmitRegistration(YTPSContext context, LoginModel loginData)
        {
            var user = await _loginRepository.GetUserByUserName(context, loginData.UserName);
            if (user == null) 
            {

                // Hashpassword first
                var salt = GenerateSalt(16);
                var hashedP = HashPassword(loginData.Password, salt);

                user = new User
                {
                    UserName = loginData.UserName,
                    Email = loginData.UserName,
                    AccountStatusNavigation = context.Accountstatuses.Where(x => x.StatusName == "Active").First(),
                    RoleNavigation = context.Roles.Where(x => x.RoleName == "Standard").First(),
                    AuthenticationNavigation = new Userauthentication
                    {
                        Password = hashedP,
                        PasswordSalt = salt,
                    }
                };

                user = await _loginRepository.AddAccount(context, user);
                await context.SaveChangesAsync();

                return new UserDTO
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                    ProfilePicture = user.ProfilePicture,
                    Role = user.RoleNavigation.RoleName,
                }; ;
            }

            return null;
        }
    }

    public interface ILoginService
    {
        Task<UserDTO> AttemptLogin(YTPSContext context, LoginModel loginData);
        Task<UserDTO> SubmitRegistration(YTPSContext context, LoginModel loginData);
        Task UpdateRefreshToken(YTPSContext context, UserDTO userDTO);
    }

    public class LoginModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
        public string RefreshToken { get; internal set; }
    }
}
