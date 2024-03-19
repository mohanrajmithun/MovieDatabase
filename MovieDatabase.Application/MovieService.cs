using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.core;


namespace MovieDatabase.Application
{
    public class MovieService: IMovieService
    {
        private readonly IMovieRepository movieRepository;

        public MovieService(IMovieRepository movieRepository) 
        {

            this.movieRepository = movieRepository;
        }

        public Task<List<Movie>> GetAllMovies() 
        {
            return movieRepository.GetAllMovies();
        }

        public Task<List<MovieWithGenresDTO>> GetAllMoviesWithGenres()
        {
            return movieRepository.GetMovieGenres();
        }

        public Task<List<Genre>> GetAllGenres()
        {
            return movieRepository.GetAllGenres();
        }

        public Task<Movie> GetMovieById(int id)
        {
            return movieRepository.GetMovieById(id);

        }

        public async Task<Movie> GetMovieByName(string name)
        {
            return await movieRepository.GetMovieByName(name);
        }


        public Task<Genre> GetGenreById(int id)
        {
            return movieRepository.GetGenreById(id);

        }

        public async Task<Genre> GetGenreByName(string name)
        {
            return await movieRepository.GetGenreByName(name);
        }

        public async Task<List<GenrewithMoviesDTO>> GetGenresWithMovies()
        {
            return await movieRepository.GetGenresWithMovies(); 
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


            return await movieRepository.AddGenreToMovie(movieGenre);
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
