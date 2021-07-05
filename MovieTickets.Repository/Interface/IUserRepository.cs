using MovieTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<MoviesUser> GetAll();
        MoviesUser Get(string Id);
        void Insert(MoviesUser entity);
        void Update(MoviesUser entity);
        void Delete(MoviesUser entity);
    }
}
