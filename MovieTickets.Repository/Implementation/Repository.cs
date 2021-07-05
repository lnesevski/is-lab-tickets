using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieTickets.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.entities = context.Set<T>();
        }

        public void Delete(T entity)
        {
            this.Remove(entity);
        }

        public T Get(Guid? Id)
        {
            return entities.SingleOrDefault(s => s.Id == Id);
        }

        public IEnumerable<T> GetAll()
        {
            return this.entities.AsEnumerable();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("entity");
            }
            this.entities.Add(entity);
            _context.SaveChanges();

        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.entities.Remove(entity);
            this._context.SaveChanges();
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void Update(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this.entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
