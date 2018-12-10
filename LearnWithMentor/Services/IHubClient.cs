using System.Threading.Tasks;

namespace LearnWithMentor.Services
{
    public interface IHubClient
    {
        Task Notify();
    }
}
