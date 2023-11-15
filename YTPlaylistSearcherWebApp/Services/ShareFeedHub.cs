using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;

namespace YTPlaylistSearcherWebApp.Services
{
    public sealed class ShareFeedHub: Hub, IShareFeedHub
    {
        static readonly Dictionary<string, string> Users = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
        }

        public async Task NewPost(SharedPostDTO post)
        {
            await Clients.All.SendAsync(WebSocketActions.NEW_POST, post);
        }

        public struct WebSocketActions
        {
            public static readonly string NEW_POST = "NewPost";
            public static readonly string TEST = "Test";
            public static readonly string DELETE_POST = "DeletePost";
        }
    }

    public interface IShareFeedHub
    {
        Task NewPost(SharedPostDTO post);
    }
}
