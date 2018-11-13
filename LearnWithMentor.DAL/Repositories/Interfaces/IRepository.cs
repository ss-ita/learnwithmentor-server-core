using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task AddAsync(T item);
        Task UpdateAsync(T item);
        Task RemoveAsync(T item);
    }
}
