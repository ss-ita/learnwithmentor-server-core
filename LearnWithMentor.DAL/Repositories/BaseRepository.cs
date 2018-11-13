﻿using System.Collections.Generic;
using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly LearnWithMentorContext Context;
        public BaseRepository(LearnWithMentorContext context)
        {
            Context = context;
        }

        public async System.Threading.Tasks.Task<IEnumerable<T>> GetAll()
        {
            return Context.Set<T>();
        }

        public async System.Threading.Tasks.Task AddAsync(T item)
        {
            Context.Set<T>().Add(item);
        }

        public async System.Threading.Tasks.Task UpdateAsync(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }

        public async System.Threading.Tasks.Task RemoveAsync(T item)
        {
            Context.Set<T>().Remove(item);
        }
    }
}
