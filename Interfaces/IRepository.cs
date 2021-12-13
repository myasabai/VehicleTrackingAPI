using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleAPI.Interfaces
{
  public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        Task<T> Insert(T entity);
        Task<T> Update(T entity);
        void Dispose(bool disposing);
    }
}
