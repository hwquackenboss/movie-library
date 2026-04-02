using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<ActionResult<ICollection<Movie>>> GetMovies()
        {
            return Ok(await _context.Movies.ToListAsync());
        }

        [HttpGet("{movieId}")]
        public async Task<ActionResult<Movie>> GetMovieByID(int movieId)
        {
            Movie? movie = await _context.Movies.FindAsync(movieId);
            if (null == movie)
            {
                return NotFound($"No such movie with ID {movieId}");
            }

            return Ok(movie);
        }

        [HttpGet("search")]
        public async Task<ActionResult<ICollection<Movie>>> GetMoviesBySearch([FromQuery] string? title)
        {
            if (null == title)
            {
                return Ok(new List<Movie>());
            }

            List<Movie>? movies = await _context.Movies
                .Where(m => m.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();

            return Ok(movies);
        }
    }
}
