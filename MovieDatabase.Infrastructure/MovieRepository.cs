using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.core;
using MovieDatabase.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace MovieDatabase.Infrastructure
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext appDbContext;



        public MovieRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<Movie>> GetAllMovies()

        {
            var Movies = await appDbContext.Movies.ToListAsync();

            return Movies;

        }

        public async Task<List<MovieWithGenresDTO>> GetMovieGenres()
        {
            List<MovieWithGenresDTO> moviesWithGenres = await appDbContext.Movies
            .Select(movie => new MovieWithGenresDTO
            {
                MovieName = movie.Name,
                Genres = movie.MovieGenres.Select(movieGenre => movieGenre.Genre.Name).ToList()
            })
            .ToListAsync();

            return moviesWithGenres;



        }

        public async Task<List<Genre>> GetAllGenres()
        {
            var genresWithMovies = await appDbContext.Genres.ToListAsync();

            return genresWithMovies;
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var Movie = await appDbContext.Movies.SingleOrDefaultAsync(movie => movie.Id == id);

            return Movie;
        }

        public async Task<Movie> GetMovieByName(string name)
        {
            var Movie = await appDbContext.Movies.SingleOrDefaultAsync(movie => movie.Name.ToUpper() == name.ToUpper());

            return Movie;
        }


        public async Task<Genre> GetGenreById(int id)
        {
            var genre = await appDbContext.Genres.SingleOrDefaultAsync(g => g.Id == id);
            return genre;
        }

        public async Task<Genre> GetGenreByName(string name)
        {
            var genre = await appDbContext.Genres.SingleOrDefaultAsync(g => g.Name == name);

            return genre;
        }


        public async Task<List<GenrewithMoviesDTO>> GetGenresWithMovies()
        {
            var genreswithmovies = await appDbContext.Genres
                                   .Select(genre => new GenrewithMoviesDTO
                                   { Genre = genre.Name,
                                       Movies = genre.MovieGenres.Select(moviegenre => moviegenre.Movie.Name).ToList() }
                                   )
                                   .ToListAsync();

            return genreswithmovies;
        }

        public async Task<CreateMovieResponseDTO> CreateMovie(CreateMovieDTO newmovie)
        {



            Movie movie = new Movie()
            {
                Name = newmovie.Name,
                Description = newmovie.Description,
                rating = newmovie.Rating
            };

            var movieresult = await appDbContext.Movies.AddAsync(movie);
            await appDbContext.SaveChangesAsync();

            CreateMovieResponseDTO createmovieresponseDTO = new CreateMovieResponseDTO()
            {
                MovieName = movieresult.Entity.Name,
                Description = movieresult.Entity.Description,
                Rating = movieresult.Entity.rating
            };


            return createmovieresponseDTO;


        }


        public async Task<Genre> createGenre(Genre genre)
        {
            var genreresult = await appDbContext.Genres.AddAsync(genre);
            await appDbContext.SaveChangesAsync();

            return genreresult.Entity;
        }

        public async Task<MovieWithGenresDTO> AddGenreToMovie(MovieGenre movieGenre)
        {


            var MovieGenreResult = await appDbContext.MovieGenres.AddAsync(movieGenre);
            var result = await appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                
                    Movie movie = await GetMovieById(movieGenre.MovieId);
                    List<MovieWithGenresDTO> moviewithgenredto = await GetMovieGenres();

                    MovieWithGenresDTO moviegenre = moviewithgenredto.FirstOrDefault(m => m.MovieName == movie.Name);

                    return moviegenre;

               

            }


            return null;
        }


        public async Task<Movie> UpdateMovie(Movie newmovie)
        {
            var movie = await appDbContext.Movies.FindAsync(newmovie.Id);

            movie.Name = newmovie.Name;
            movie.Description = newmovie.Description;
            movie.rating = newmovie.rating;

            await appDbContext.SaveChangesAsync(); 

            return movie;

        }

        public async Task<MovieWithGenresDTO> DeleteGenretoMovie(MovieGenre movieGenre)
        {

            var MovieGenreResult = appDbContext.MovieGenres.Remove(movieGenre);
            var result = await appDbContext.SaveChangesAsync();

            if (result > 0) {
                Movie movie = await GetMovieById(movieGenre.MovieId);
                List<MovieWithGenresDTO> moviewithgenredto = await GetMovieGenres();

                var moviegenre = moviewithgenredto.FirstOrDefault(m => m.MovieName == movie.Name);

                return moviegenre;
            }

            return null;

           
        }

        public async Task<Movie> DeleteMovie(int Id)
        {
            var movieToDelete = await appDbContext.Movies.FindAsync(Id);

            if (movieToDelete != null)
            {
                appDbContext.Movies.Remove(movieToDelete);
                var result = await appDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return movieToDelete;
                }

             }

            return null;
        }
      

        
    }
}

