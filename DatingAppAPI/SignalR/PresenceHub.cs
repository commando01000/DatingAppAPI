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
        public PresenceHub(IAccountService _accountService)
        {
            this._accountService = _accountService;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("UserIsOnline", await this._accountService.GetCurrentUserDisplayName());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Others.SendAsync("UserIsOffline", await this._accountService.GetCurrentUserDisplayName());
            await base.OnDisconnectedAsync(exception);
        }
    }
}
