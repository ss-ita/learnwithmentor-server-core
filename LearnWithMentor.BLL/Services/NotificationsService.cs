using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentor.DAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentor.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    class NotificationsService : BaseService
    {
        public NotificationsService(IUnitOfWork db) : base(db)
        {

        }
    }
}
