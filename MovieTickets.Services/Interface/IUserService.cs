using MovieTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Services.Interface
{
    public interface IUserService
    {
        MoviesUser Get(string Id);
        void Update(MoviesUser user);
        List<MoviesUser> GetAll();

    }
}
