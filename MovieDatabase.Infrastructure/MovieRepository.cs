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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Caching.Memory;

namespace MovieDatabase.Infrastructure
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger<MovieRepository> logger;
        private readonly IMemoryCache memoryCache;

        public MovieRepository(AppDbContext appDbContext,ILogger<MovieRepository> logger )
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }

        public async Task<List<Movie>> GetAllMovies()

        {
            try
            {

                logger.LogInformation("Fetching all movies...");
                var Movies = await appDbContext.Movies.ToListAsync();

                return Movies;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching all movies.");

                throw;
            }

        }

        public async Task<List<MovieWithGenresDTO>> GetMovieGenres()
        {
            try
            {
                logger.LogInformation("Fetching all movies with their genres...");
                List<MovieWithGenresDTO> moviesWithGenres = await appDbContext.Movies
                                        .Select(movie => new MovieWithGenresDTO
                                                    {
                                                        MovieName = movie.Name,
                                                        Genres = movie.MovieGenres.Select(movieGenre => movieGenre.Genre.Name).ToList()
                                                    })
                                    .ToListAsync();

                return moviesWithGenres;


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching all movies with their genres.");

                throw;
            }




        }

        public async Task<List<Genre>> GetAllGenres()
        {
            try
            {
                logger.LogInformation("Fetching all genres...");

                var genresWithMovies = await appDbContext.Genres.ToListAsync();

                return genresWithMovies;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching all genres.");


                throw;
            }

        }

        public async Task<Movie> GetMovieById(int id)
        {
            try
            {
                logger.LogInformation("Fetching movie by ID...");

                var Movie = await appDbContext.Movies.SingleOrDefaultAsync(movie => movie.Id == id);

                return Movie;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching movie by ID.");


                throw;
            }

        }

        public async Task<Movie> GetMovieByName(string name)
        {
            try
            {
                logger.LogInformation("Fetching movie by name...");
                var Movie = await appDbContext.Movies.SingleOrDefaultAsync(movie => movie.Name.ToUpper() == name.ToUpper());

            return Movie;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching movie by name.");


                throw;
            }
        }


        public async Task<Genre> GetGenreById(int id)
        {
            try
            {
                logger.LogInformation("Fetching Genre by ID...");

                var genre = await appDbContext.Genres.SingleOrDefaultAsync(g => g.Id == id);

                return genre;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching Genre by ID.");


                throw;
            }
        }

        public async Task<Genre> GetGenreByName(string name)
        {
            try
            {
                logger.LogInformation("Fetching Genre by name...");
                var genre = await appDbContext.Genres.SingleOrDefaultAsync(g => g.Name == name);

                return genre;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching Genre by name.");


                throw;
            }
        }


        public async Task<List<GenrewithMoviesDTO>> GetGenresWithMovies()
        {
            try
            {
                logger.LogInformation("Fetching Genres with their movies...");
                var genreswithmovies = await appDbContext.Genres
                                   .Select(genre => new GenrewithMoviesDTO
                                   { Genre = genre.Name,
                                       Movies = genre.MovieGenres.Select(moviegenre => moviegenre.Movie.Name).ToList() }
                                   )
                                   .ToListAsync();

                return genreswithmovies;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Genres with their movies.");


                throw;
            }
        }

        public async Task<CreateMovieResponseDTO> CreateMovie(CreateMovieDTO newmovie)
        {

            try
            {
                logger.LogInformation("Creating a New Movie");

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
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating the movie.");


                throw;
            }

        }


        public async Task<Genre> createGenre(Genre genre)
        {
            try
            {
                logger.LogInformation("Creating a New Genre");
                var genreresult = await appDbContext.Genres.AddAsync(genre);
                await appDbContext.SaveChangesAsync();

                return genreresult.Entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating New Genre.");


                throw;
            }
        }

        public async Task<MovieWithGenresDTO> AddGenreToMovie(MovieGenre movieGenre)
        {


            try
            {
                logger.LogInformation("Adding Genre to the Movie");
                var MovieGenreResult = await appDbContext.MovieGenres.AddAsync(movieGenre);
                var result = await appDbContext.SaveChangesAsync();

                if (result > 0)
                {

                    Movie movie = await GetMovieById(movieGenre.MovieId);
                    List<MovieWithGenresDTO> moviewithgenredto = await GetMovieGenres();

                    MovieWithGenresDTO moviegenre = moviewithgenredto.FirstOrDefault(m => m.MovieName == movie.Name);

                    return moviegenre;



                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Adding Genre to the Movie");

                throw;
            }

            return null;
        }


        public async Task<Movie> UpdateMovie(Movie newmovie)
        {
            try
            {
                logger.LogInformation("Updating a existing Movie");
                var movie = await appDbContext.Movies.FindAsync(newmovie.Id);

                movie.Name = newmovie.Name;
                movie.Description = newmovie.Description;
                movie.rating = newmovie.rating;

                await appDbContext.SaveChangesAsync();

                return movie;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Updating a existing Movie");

                throw;
            }

        }

        public async Task<MovieWithGenresDTO> DeleteGenretoMovie(MovieGenre movieGenre)
        {
            try
            {
                    logger.LogInformation("Deleting Genre to the Movie");

                    var MovieGenreResult = appDbContext.MovieGenres.Remove(movieGenre);
                    var result = await appDbContext.SaveChangesAsync();

                    if (result > 0) {
                        Movie movie = await GetMovieById(movieGenre.MovieId);
                        List<MovieWithGenresDTO> moviewithgenredto = await GetMovieGenres();

                        var moviegenre = moviewithgenredto.FirstOrDefault(m => m.MovieName == movie.Name);

                        return moviegenre;
                    }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Deleting Genre to the Movie");

                throw;
            }

            return null;

           
        }

        public async Task<Movie> DeleteMovie(int Id)
        {
            try
            {
                logger.LogInformation("Deleting a existing Movie");
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
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while Deleting a Movie");

                throw;
            }


            return null;
        }
      

        
    }
}

