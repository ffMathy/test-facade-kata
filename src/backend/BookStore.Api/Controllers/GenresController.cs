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
    public async Task<IEnumerable<GetAllGenresResponse>> GetAll() =>
        (await db.Genres.ToListAsync())
            .Select(g => new GetAllGenresResponse(g.Id, g.Name));

    // GET /api/genres/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<GetGenreByIdResponse>> GetById(int id)
    {
        var genre = await db.Genres.FindAsync(id);
        return genre is null ? NotFound() : Ok(new GetGenreByIdResponse(genre.Id, genre.Name));
    }

    // POST /api/genres
    [HttpPost]
    public async Task<ActionResult<CreateGenreResponse>> Create(CreateGenreRequest request)
    {
        var genre = new Genre { Name = request.Name };
        await db.Genres.AddAsync(genre);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = genre.Id },
            new CreateGenreResponse(genre.Id, genre.Name));
    }

    // PUT /api/genres/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateGenreRequest request)
    {
        var existing = await db.Genres.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Name = request.Name;

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

    // ── Request / Response records ──────────────────────────────────────────────
    // Each endpoint has its own dedicated type — even when two types share the
    // same fields they are never reused, so each endpoint can evolve independently.

    /// <summary>Payload for creating a new genre.</summary>
    public record CreateGenreRequest(string Name);

    /// <summary>Payload for replacing a genre's mutable fields.</summary>
    public record UpdateGenreRequest(string Name);

    /// <summary>Response for GET /api/genres — one item per genre in the list.</summary>
    public record GetAllGenresResponse(int Id, string Name);

    /// <summary>Response for GET /api/genres/{id}.</summary>
    public record GetGenreByIdResponse(int Id, string Name);

    /// <summary>Response for POST /api/genres — contains the newly created genre.</summary>
    public record CreateGenreResponse(int Id, string Name);
}
