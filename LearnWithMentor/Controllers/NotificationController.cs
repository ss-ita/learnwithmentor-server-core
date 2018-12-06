using LearnWithMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace LearnWithMentor.Controllers
{
    [Authorize]
    public class NotificationController : Hub<IHubClient>
    {
        public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            string userName = Context.User.Identity.Name;
            ConnectedUsers.TryAdd(Context.User.Identity.Name, Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception ex)
        {
            string userName = Context.User.Identity.Name;
            string removedValue = "";
            ConnectedUsers.TryRemove(userName, out removedValue);   
            return base.OnDisconnectedAsync(ex);
        }
    }
}
