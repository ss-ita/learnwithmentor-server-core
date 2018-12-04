using LearnWithMentor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LearnWithMentor.Controllers
{
    [AllowAnonymous]
    public class NotificationController : Hub<IHubClient>
    {

    }
}
