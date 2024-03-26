using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Caching.Distributed;
using MovieDatabase.core;


namespace MovieDatabase.Application
{
    public class MovieService: IMovieService
    {
        private readonly IMovieRepository movieRepository;
        private readonly IDistributedCache cache;

        public MovieService(IMovieRepository movieRepository, IDistributedCache cache) 
        {

            this.movieRepository = movieRepository;
            this.cache = cache;
        }

        public async Task<List<Movie>> GetAllMovies() 
        {
            string cacheKey = "AllMovies";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            List<Movie> Movies = new List<Movie>();

            if(cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                Movies = JsonSerializer.Deserialize<List<Movie>>(cachedDataString);

            }
            else
            {
                Movies =  await movieRepository.GetAllMovies();

                string cachedDataString = JsonSerializer.Serialize(Movies);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }

            return Movies;

        }

        public async Task<List<MovieWithGenresDTO>> GetAllMoviesWithGenres()
        {
            string cacheKey = "AllMoviesWithGenres";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            List<MovieWithGenresDTO> MovieswithGenres = new List<MovieWithGenresDTO>();

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                MovieswithGenres = JsonSerializer.Deserialize<List<MovieWithGenresDTO>>(cachedDataString);

            }
            else
            {
                MovieswithGenres = await movieRepository.GetMovieGenres();

                string cachedDataString = JsonSerializer.Serialize(MovieswithGenres);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }

            return MovieswithGenres;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            string cacheKey = "AllGenres";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            List<Genre> Genres = new List<Genre>();

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                Genres = JsonSerializer.Deserialize<List<Genre>>(cachedDataString);

            }
            else
            {
                Genres = await movieRepository.GetAllGenres();

                string cachedDataString = JsonSerializer.Serialize(Genres);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }

            return Genres;
        }

        public async Task<Movie> GetMovieById(int id)
        {
            string cacheKey = $"movie-id:{id}";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            Movie ExistingMovie = new Movie();

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                ExistingMovie = JsonSerializer.Deserialize<Movie>(cachedDataString);

            }
            else
            {
                ExistingMovie = await movieRepository.GetMovieById(id);

                string cachedDataString = JsonSerializer.Serialize(ExistingMovie);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }
            return ExistingMovie;

        }

        public async Task<Movie> GetMovieByName(string name)
        {
            string cacheKey = $"movie-id:{name}";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            Movie ExistingMovie = new Movie();

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                ExistingMovie = JsonSerializer.Deserialize<Movie>(cachedDataString);

            }
            else
            {
                ExistingMovie = await movieRepository.GetMovieByName(name);

                string cachedDataString = JsonSerializer.Serialize(ExistingMovie);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }
            return ExistingMovie;
        }


        public async Task<Genre> GetGenreById(int id)
        {
            string cacheKey = $"Genre-id:{id}";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            Genre ExistingGenre = new Genre();

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                ExistingGenre = JsonSerializer.Deserialize<Genre>(cachedDataString);

            }
            else
            {
                ExistingGenre = await movieRepository.GetGenreById(id);

                string cachedDataString = JsonSerializer.Serialize(ExistingGenre);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }
            return ExistingGenre;

        }

        public async Task<Genre> GetGenreByName(string name)
        {
            string cacheKey = $"Genre-id:{name}";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            Genre ExistingGenre = new Genre();

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                ExistingGenre = JsonSerializer.Deserialize<Genre>(cachedDataString);

            }
            else
            {
                ExistingGenre = await movieRepository.GetGenreByName(name);

                string cachedDataString = JsonSerializer.Serialize(ExistingGenre);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }
            return ExistingGenre;
        }

        public async Task<List<GenrewithMoviesDTO>> GetGenresWithMovies()
        {
            string cacheKey = $"GenreswithMovies";

            Byte[] cachedData = await cache.GetAsync(cacheKey);
            List<GenrewithMoviesDTO> GenreswithMovies = new List<GenrewithMoviesDTO>();

            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                GenreswithMovies = JsonSerializer.Deserialize<List<GenrewithMoviesDTO>>(cachedDataString);

            }
            else
            {
                GenreswithMovies = await movieRepository.GetGenresWithMovies();

                string cachedDataString = JsonSerializer.Serialize(GenreswithMovies);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await cache.SetAsync(cacheKey, dataToCache, options);


            }
            return GenreswithMovies; 
        }


        public async Task<CreateMovieResponseDTO> CreateMovie(CreateMovieDTO newmovie)
        {
            return await movieRepository.CreateMovie(newmovie);
        }

        public async Task<Genre> createGenre(Genre genre)
        {
            return await movieRepository.createGenre(genre);
        }

        public async Task<MovieWithGenresDTO> AddGenreToMovie(MovieGenre movieGenre)
        {


            //try
            {
                return await movieRepository.AddGenreToMovie(movieGenre);

            }
            //catch (Exception)
            //{

            //    throw;
            //}
            }

        public async Task<Movie> UpdateMovie(Movie newmovie)
        {
            return await movieRepository.UpdateMovie(newmovie);
        }

        public async Task<MovieWithGenresDTO> DeleteGenretoMovie(MovieGenre movieGenre)
        {
            return await movieRepository.DeleteGenretoMovie(movieGenre);
        }

        public async Task<Movie> DeleteMovie(int Id)
        {
            return await movieRepository.DeleteMovie(Id);
        }

    }
}
