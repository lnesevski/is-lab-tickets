using MovieTickets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Services.Interface
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        Movie GetDetailsForMovie(Guid? Id);
        Movie GetDetailsForMovie(string MovieName);
        void CreateNewMovie(Movie m);
        void UpdateExistingMovie(Movie m);
        void DeleteMovie(Guid Id);
        bool MovieExists(Guid Id);
    }
}
