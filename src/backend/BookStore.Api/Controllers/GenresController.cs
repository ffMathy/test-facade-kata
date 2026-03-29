using BookStore.Data;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Controllers;

/// <summary>
/// CRUD endpoints for Genres.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GenresController(BookStoreDbContext db) : ControllerBase
{
    // GET /api/genres
    [HttpGet]
    public async Task<IEnumerable<Genre>> GetAll() =>
        await db.Genres.ToListAsync();

    // GET /api/genres/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetById(int id)
    {
        var genre = await db.Genres.FindAsync(id);
        return genre is null ? NotFound() : Ok(genre);
    }

    // POST /api/genres
    [HttpPost]
    public async Task<ActionResult<Genre>> Create(Genre genre)
    {
        db.Genres.Add(genre);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
    }

    // PUT /api/genres/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Genre genre)
    {
        if (id != genre.Id) return BadRequest();

        db.Entry(genre).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/genres/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await db.Genres.FindAsync(id);
        if (genre is null) return NotFound();

        db.Genres.Remove(genre);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
