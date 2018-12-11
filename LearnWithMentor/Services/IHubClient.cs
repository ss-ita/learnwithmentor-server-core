using System.Threading.Tasks;

namespace LearnWithMentor.Services
{
    public interface IHubClient
    {
        Task Notify();
        Task SendMessage(string name, string message, string timeSent);
    }
}
