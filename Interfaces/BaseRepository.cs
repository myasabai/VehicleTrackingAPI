using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleAPI.Models;

namespace VehicleAPI.Interfaces
{
    public class BaseRepository<T> : IRepository<T> where T:class
    {
        public VehicleTrackingContext _context;
        public BaseRepository(VehicleTrackingContext ctx)
        {
            _context = ctx;
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            this.Dispose(disposing);
        }

        public  IQueryable<T> GetAll()
        {
            return  _context.Set<T>().AsQueryable().AsNoTracking();
        }

        public async Task<T> Insert(T entity)
        {
            T insertedentity = _context.Set<T>().Add(entity).Entity;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return insertedentity;
            }
            else
            {
                return null;
            }
        }

        public async Task<T> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }
    }
}
