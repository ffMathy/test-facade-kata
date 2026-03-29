using BookStore.Data;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Controllers;

/// <summary>
/// CRUD endpoints for Books.
/// A book belongs to an Author and can have many Reviews and Genres.
/// Chain: Author -> Book -> Review
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(BookStoreDbContext db) : ControllerBase
{
    // GET /api/books — includes Author and Genres for a richer response
    [HttpGet]
    public async Task<IEnumerable<Book>> GetAll() =>
        await db.Books
            .Include(b => b.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .ToListAsync();

    // GET /api/books/{id} — also loads reviews for the detail view
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        var book = await db.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);

        return book is null ? NotFound() : Ok(book);
    }

    // POST /api/books
    [HttpPost]
    public async Task<ActionResult<Book>> Create(Book book)
    {
        db.Books.Add(book);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT /api/books/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.Id) return BadRequest();

        db.Entry(book).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/books/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await db.Books.FindAsync(id);
        if (book is null) return NotFound();

        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
