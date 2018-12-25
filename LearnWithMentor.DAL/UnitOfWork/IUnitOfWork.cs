using System;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICommentRepository Comments { get; }
        IGroupRepository Groups { get; }
        IMessageRepository Messages { get; }
        IPlanRepository Plans { get; }
        IPlanSuggestionRepository PlanSuggestions { get; }
        IPlanTaskRepository PlanTasks { get; }
        IRoleRepository Roles { get; }
        ISectionRepository Sections { get; }
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        IUserTaskRepository UserTasks { get; }
        INotificationRepository Notification { get; }
        IGroupChatRepository GroupChatMessage { get; }
        void Save();
    }
}
