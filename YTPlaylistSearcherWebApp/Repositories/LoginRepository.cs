using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;
using YTPlaylistSearcherWebApp.Services;

namespace YTPlaylistSearcherWebApp.Repositories
{
    public class LoginRepository : ILoginRepository
    {

        public LoginRepository()
        {

        }

        public async Task<User> AddAccount(YTPSContext context, User user)
        {
            await context.AddAsync(user);

            return user;
        }

        public async Task<User> GetUserByUserName(YTPSContext context, string username)
        {
            var user = context.Users
                .Include(x => x.AuthenticationNavigation)
                .Include(x => x.AccountStatusNavigation)
                .Include(x => x.RoleNavigation)
                .ThenInclude(x => x.Rolepolicies)
                .Where(x => x.UserName == username)
                .FirstOrDefault();

            return user;
        }

        public Task UpdateRefreshToken(YTPSContext context, UserDTO userDTO)
        {
            var auth = context.Users
                .Include(x => x.AuthenticationNavigation)
                .Where(x => x.Id == userDTO.UserID)
                .Select(x => x.AuthenticationNavigation)
                .FirstOrDefault();

            if (auth != null)
            {
                auth.RefreshDate = userDTO.RefreshTokenExpiryTime;
                auth.RefreshToken = userDTO.RefreshToken;
            }

            return Task.CompletedTask;
        }
    }

    public interface ILoginRepository
    {
        Task<User> AddAccount(YTPSContext context, User user);
        Task<User> GetUserByUserName(YTPSContext context, String username);
        Task UpdateRefreshToken(YTPSContext context, UserDTO userDTO);
    }
}
