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
    public async Task<ActionResult<IEnumerable<Genre>>> GetAll(int bookId)
    {
        if (!await db.Books.AnyAsync(b => b.Id == bookId))
            return NotFound($"Book {bookId} not found.");

        return await db.BookGenres
            .Where(bg => bg.BookId == bookId)
            .Select(bg => bg.Genre!)
            .ToListAsync();
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

        db.BookGenres.Add(new BookGenre { BookId = bookId, GenreId = genreId });
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
}
