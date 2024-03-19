using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieDatabase.core;

namespace MovieDatabase.Application
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllMovies();

        Task<List<MovieWithGenresDTO>> GetAllMoviesWithGenres();

        Task<List<Genre>> GetAllGenres();

        Task<Movie> GetMovieById(int id);

        Task<Movie> GetMovieByName(string name);

        Task<Genre> GetGenreById(int id);

        Task<Genre> GetGenreByName(string name);

        Task<List<GenrewithMoviesDTO>> GetGenresWithMovies();

        Task<CreateMovieResponseDTO> CreateMovie(CreateMovieDTO newmovie);

        Task<Genre> createGenre(Genre genre);

        Task<MovieWithGenresDTO> AddGenreToMovie(MovieGenre movieGenre);

        Task<Movie> UpdateMovie(Movie newmovie);

        Task<MovieWithGenresDTO> DeleteGenretoMovie(MovieGenre movieGenre);

        Task<Movie> DeleteMovie(int Id);




    }
}
