using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DAL.Interfaces
{
    public interface IRepository<T>
    {
        Task Create(T entity);
        void Update(T entity);
        Task Delete(T entity);
        IEnumerable<T> GetAll();
    }
}
