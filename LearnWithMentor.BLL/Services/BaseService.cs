﻿using LearnWithMentorBLL.Interfaces;
using LearnWithMentor.DAL.UnitOfWork;

namespace LearnWithMentorBLL.Services
{
    public class BaseService: IDisposableService
    {
        protected readonly IUnitOfWork db;
        public BaseService(IUnitOfWork db)
        {
            this.db = db;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
