using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository;
using MovieTickets.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieTickets.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Movie> movies;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
            this.movies = context.Set<Movie>();
        }

        public void CreateNewMovie(Movie m)
        {
            if (m == null)
            {
                throw new ArgumentNullException("movie");
            }
            _context.Add(m);
            _context.SaveChanges();
        }

        public void DeleteMovie(Guid Id)
        {
            var movie = movies.SingleOrDefault(m => m.Id == Id);
            this._context.Remove(movie);
            _context.SaveChanges();
        }

        public List<Movie> GetAllMovies()
        {
            return this.movies.ToList();
        }

        public Movie GetDetailsForMovie(Guid? Id)
        {
            var movie = this.movies.SingleOrDefault(m => m.Id == Id);
            return movie;
        }

        public Movie GetDetailsForMovie(string MovieName)
        {
            return this.movies.SingleOrDefault(m => m.MovieName == MovieName);
        }

        public bool MovieExists(Guid Id)
        {
            return this.movies.Any(m => m.Id == Id);
        }

        public void UpdateExistingMovie(Movie m)
        {
            if (m == null)
            {
                throw new ArgumentNullException("movie");
            }
            this._context.Movies.Update(m);
            _context.SaveChanges();
        }
    }
}
