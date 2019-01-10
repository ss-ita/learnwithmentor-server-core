using System.Collections.Generic;
using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using LearnWithMentor.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly LearnWithMentorContext Context;
        public BaseRepository(LearnWithMentorContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return Context.Set<T>();
        }

        public async Task AddAsync(T item)
        {
            Context.Set<T>().Add(item);
        }

        public async Task UpdateAsync(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }

        public async Task RemoveAsync(T item)
        {
            Context.Set<T>().Remove(item);
        }
    }
}
