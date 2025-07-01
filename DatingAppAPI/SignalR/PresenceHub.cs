using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Services.Layer.Identity;
using System.Security.Claims;

namespace DatingAppAPI.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly IAccountService _accountService;
        private readonly PresenceTracker _presence;
        public PresenceHub(IAccountService _accountService, PresenceTracker presence)
        {
            this._accountService = _accountService;
            this._presence = presence;
        }
        public override async Task OnConnectedAsync()
        {
            if (Context.User == null) throw new HubException("User not found");

            await _presence.UserConnected(await this._accountService.GetCurrentUserDisplayName(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", await this._accountService.GetCurrentUserDisplayName());
            var CurrentUsers = await _presence.GetOnlineUsers();

            await Clients.All.SendAsync("GetOnlineUsers", CurrentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User == null) throw new HubException("User not found");
            await this._presence.UserDisconnected(await this._accountService.GetCurrentUserDisplayName(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", await this._accountService.GetCurrentUserDisplayName());
            await base.OnDisconnectedAsync(exception);
        }
    }
}
