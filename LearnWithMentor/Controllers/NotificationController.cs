using LearnWithMentor.Services;
using LearnWithMentorBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace LearnWithMentor.Controllers
{
    [Authorize]
    public class NotificationController : Hub<IHubClient>
    {
        private readonly INotificationService notificationService;

        public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        public NotificationController(INotificationService notificationService) : base()
        {
            this.notificationService = notificationService;
        }

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

        [HttpGet]
        [Route("api/notifications/{userId}")]
        public async Task<ActionResult> GetNotificationsAsync(int userId)
        {
            var notifications = await notificationService.GetNotificationsAsync(userId, 5);
            return new JsonResult(notifications);
        }

        [HttpPost]
        [Route("api/notifications/{userId}")]
        public async Task<IActionResult> MarkAllNotificationsAsReadAsync(int userId)
        {
            await notificationService.MarkAllNotificationsAsReadAsync(userId);
            return new OkResult();
        }
    }
}
