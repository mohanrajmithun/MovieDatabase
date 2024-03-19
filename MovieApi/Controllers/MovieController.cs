using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Application;
using MovieDatabase.core;
using MovieDatabase.Infrastructure;
using System.Xml.Linq;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService movieService;
        private readonly AppDbContext appDbContext;

        public MovieController(IMovieService movieService, AppDbContext appDbContext) 
        {
            this.movieService = movieService;
            this.appDbContext = appDbContext;
        }

        [Authorize]
        [HttpGet("movies")]
        public async Task<ActionResult<IList<Movie>>> GetAllMovies() 
        {
            try
            {
                var Movies = await movieService.GetAllMovies();
                return Ok(Movies);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");

            }
            

        }
        [Authorize]
        [HttpGet("movieswithgenres")]
        public async Task<ActionResult<List<MovieWithGenresDTO>>> GetAllMoviesWithGenres()
        {
            try
            {
                var moviewithgenre = await movieService.GetAllMoviesWithGenres();
                return Ok(moviewithgenre);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");

            }

           
        }
        [Authorize]
        [HttpGet("Genres")]
        public async Task<ActionResult<List<Genre>>> GetAllGenres()
        {
            try
            {
                var Genres = await movieService.GetAllGenres();
                return Ok(Genres);

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request");

            }
            
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            try
            {
                // Validate the input parameter
                if (id == null)
                {
                    return BadRequest("Movie name cannot be empty");
                }

                var Movie = await movieService.GetMovieById(id);
                 if (Movie == null)
                {
                    return NotFound($"Movie with name '{id}' not found");
                }

                return Ok(Movie);
            }
            catch (Exception ex)
            {
               return StatusCode(500, "An error occurred while processing your request");
            }
            
        }
        [Authorize]
        [HttpGet("{name}")]
        public async Task<ActionResult<Movie>> GetMovieByName(string name)
        {
            try
            {
                // Validate the input parameter
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("Movie name cannot be empty");
                }

                var movie = await movieService.GetMovieByName(name);
                if (movie == null)
                {
                    return NotFound($"Movie with name '{name}' not found");
                }

                return Ok(movie);
            }
            catch (Exception ex)
            {
                // Log the exception
                // You may also want to return a generic error message to the client
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
        [Authorize]
        [HttpGet("GetGenreById")]
        public async Task<ActionResult<Genre>> GetGenreById(int GenreId)
        {
            try
            {
                // Validate the input parameter
                if (GenreId == null)
                {
                    return BadRequest("GenreId cannot be empty");
                }

                var genre = await movieService.GetGenreById(GenreId);
                if (genre == null)
                {
                    return NotFound($"Genre with Id '{GenreId}' not found");
                }

                return Ok(genre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }

        }
        [Authorize]
        [HttpGet("GetGenreByName")]
        public async Task<ActionResult<Genre>> GetGenreByName(string GenreName)
        {
            try
            {
                // Validate the input parameter
                if (string.IsNullOrEmpty(GenreName))
                {
                    return BadRequest("Genre name cannot be empty");
                }

                var genre = await movieService.GetGenreByName(GenreName);
                if (genre == null)
                {
                    return NotFound($"Genre with name '{GenreName}' not found");
                }

                return Ok(genre);
            }
            catch (Exception ex)
            {
                // Log the exception
                // You may also want to return a generic error message to the client
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Authorize]
        [HttpGet("GenreWithMovies")]
        public async Task<ActionResult<List<GenrewithMoviesDTO>>> GetGenreWithMovies()
        {
            try
            {
                var genreswithmovies = await movieService.GetGenresWithMovies();

                if (genreswithmovies == null)
                {
                    return NotFound();
                }

                return Ok(genreswithmovies);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("createMovie")]
        public async Task<ActionResult<CreateMovieResponseDTO>> CreateMovie(CreateMovieDTO movie)
        {
            return await movieService.CreateMovie(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("createGenre")]
        public async Task<ActionResult<Genre>> createGenre(Genre genre)
        {
            return await movieService.createGenre(genre);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddGenreToMovie")]
        public async Task<ActionResult<MovieWithGenresDTO>> AddGenreToMovie(MovieGenre movieGenre)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    return BadRequest(errors);
                }
                var movie = await GetMovieById(movieGenre.MovieId);
                if (movie == null)
                {
                    return StatusCode(500,$"the movie with id:{movieGenre.MovieId} does not exist");
                }

                var genre = await GetGenreById(movieGenre.GenreId);
                if (genre == null)
                {
                    return StatusCode(500, $"the genre with id:{movieGenre.GenreId} does not exist");
                }

                var existingMovieGenre = await appDbContext.MovieGenres.FirstOrDefaultAsync(mg => mg.MovieId == movieGenre.MovieId && mg.GenreId == movieGenre.GenreId);

                if (existingMovieGenre != null) {

                    return StatusCode(500, $"The genre with GenreId:{movieGenre.GenreId} is already added to the Movie with MovieId:{movieGenre.MovieId}");
                }



                var moviewithgenre = await movieService.AddGenreToMovie(movieGenre);
                if (moviewithgenre != null) {
                    return Ok(moviewithgenre);
                }

                return StatusCode(500, $"Error adding the genre to the movie");

            }
            catch (Exception)
            {
                return BadRequest();
            }



        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateMovie")]
        public async Task<ActionResult<Movie>> UpdateMovie(Movie newmovie)
        {
            try
            {
                if (newmovie == null)
                {
                    return BadRequest("Movie object cannot be null");
                }

                var isMovie = await movieService.GetMovieById(newmovie.Id);

                if (isMovie == null)
                {
                    return StatusCode(500, $"the movie with id:{newmovie.Id} does not exist");
                }

                var movie = await movieService.UpdateMovie(newmovie);

                return Ok(movie);

            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
           
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteGenretoMovie")]
        public async Task<ActionResult<MovieWithGenresDTO>> DeleteGenretoMovie(MovieGenre movieGenre)
        {

            try
            {
                
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    return BadRequest(errors);
                }
                var movie = await GetMovieById(movieGenre.MovieId);
                if (movie == null)
                {
                    return StatusCode(500,$"the movie with id:{movieGenre.MovieId} does not exist");
                }

                var genre = await GetGenreById(movieGenre.GenreId);
                if (genre == null)
                {
                    return StatusCode(500, $"the genre with id:{movieGenre.GenreId} does not exist");
                }

                var moviewithgenre = await movieService.DeleteGenretoMovie(movieGenre);
                if (moviewithgenre != null) {
                    return Ok(moviewithgenre);
                }

                return StatusCode(500, $"Error deleting the genre to the movie");

            }
            catch (Exception)
            {
                return BadRequest();
            }

            }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteMovie")]
        public async Task<ActionResult<Movie>> DeleteMovie(int Id)
        {

            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    return BadRequest(errors);
                }
                var movie = await GetMovieById(Id);
                if (movie == null)
                {
                    return StatusCode(500, $"the movie with id:{Id} does not exist");
                }

                var DeletedMovie = await movieService.DeleteMovie(Id);
                if (DeletedMovie != null)
                {
                    return Ok(DeletedMovie);
                }

                return StatusCode(500, $"Error deleting the movie");

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
