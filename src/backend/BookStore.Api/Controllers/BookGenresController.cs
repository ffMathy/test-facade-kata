using BookStore.Data;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Controllers;

/// <summary>
/// Endpoints for assigning Genres to Books (the BookGenre join table).
/// Example: POST /api/books/{bookId}/genres/{genreId} links a genre to a book.
/// </summary>
[ApiController]
[Route("api/books/{bookId}/genres")]
public class BookGenresController(BookStoreDbContext db) : ControllerBase
{
    // GET /api/books/{bookId}/genres — list all genres for a book
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllBookGenresResponse>>> GetAll(int bookId)
    {
        if (!await db.Books.AnyAsync(b => b.Id == bookId))
            return NotFound($"Book {bookId} not found.");

        var genres = await db.BookGenres
            .Where(bg => bg.BookId == bookId)
            .Select(bg => new GetAllBookGenresResponse(bg.Genre!.Id, bg.Genre.Name))
            .ToListAsync();

        return Ok(genres);
    }

    // POST /api/books/{bookId}/genres/{genreId} — link a genre to a book
    [HttpPost("{genreId}")]
    public async Task<IActionResult> Add(int bookId, int genreId)
    {
        if (!await db.Books.AnyAsync(b => b.Id == bookId))
            return NotFound($"Book {bookId} not found.");

        if (!await db.Genres.AnyAsync(g => g.Id == genreId))
            return NotFound($"Genre {genreId} not found.");

        // Avoid duplicate links
        if (await db.BookGenres.AnyAsync(bg => bg.BookId == bookId && bg.GenreId == genreId))
            return Conflict("This genre is already assigned to the book.");

        await db.BookGenres.AddAsync(new BookGenre { BookId = bookId, GenreId = genreId });
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/books/{bookId}/genres/{genreId} — unlink a genre from a book
    [HttpDelete("{genreId}")]
    public async Task<IActionResult> Remove(int bookId, int genreId)
    {
        var link = await db.BookGenres
            .FirstOrDefaultAsync(bg => bg.BookId == bookId && bg.GenreId == genreId);

        if (link is null) return NotFound();

        db.BookGenres.Remove(link);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Request / Response records ──────────────────────────────────────────────
    // Each endpoint has its own dedicated type — even when two types share the
    // same fields they are never reused, so each endpoint can evolve independently.

    /// <summary>Response for GET /api/books/{bookId}/genres — one item per genre assigned to the book.</summary>
    public record GetAllBookGenresResponse(int Id, string Name);
}
