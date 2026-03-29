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
    public async Task<IEnumerable<GetAllBooksResponse>> GetAll()
    {
        var books = await db.Books
            .Include(b => b.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .ToListAsync();

        return books.Select(b => new GetAllBooksResponse(
            b.Id,
            b.Title,
            b.PublishedYear,
            b.Author is null ? null : new GetAllBooksAuthorEntry(b.Author.Id, b.Author.Name),
            b.BookGenres.Select(bg => new GetAllBooksGenreEntry(bg.Genre!.Id, bg.Genre.Name)).ToList()
        ));
    }

    // GET /api/books/{id} — also includes reviews in the detail view
    [HttpGet("{id}")]
    public async Task<ActionResult<GetBookByIdResponse>> GetById(int id)
    {
        var book = await db.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return NotFound();

        return Ok(new GetBookByIdResponse(
            book.Id,
            book.Title,
            book.PublishedYear,
            book.Author is null ? null : new GetBookByIdAuthorEntry(book.Author.Id, book.Author.Name),
            book.BookGenres.Select(bg => new GetBookByIdGenreEntry(bg.Genre!.Id, bg.Genre.Name)).ToList(),
            book.Reviews.Select(r => new GetBookByIdReviewEntry(r.Id, r.ReviewerName, r.Rating, r.Comment)).ToList()
        ));
    }

    // POST /api/books
    [HttpPost]
    public async Task<ActionResult<CreateBookResponse>> Create(CreateBookRequest request)
    {
        var book = new Book
        {
            Title = request.Title,
            PublishedYear = request.PublishedYear,
            AuthorId = request.AuthorId
        };
        await db.Books.AddAsync(book);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = book.Id },
            new CreateBookResponse(book.Id, book.Title, book.PublishedYear, book.AuthorId));
    }

    // PUT /api/books/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBookRequest request)
    {
        var existing = await db.Books.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Title = request.Title;
        existing.PublishedYear = request.PublishedYear;
        existing.AuthorId = request.AuthorId;

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

    // ── Request / Response records ──────────────────────────────────────────────
    // Each endpoint has its own dedicated type — even when two types share the
    // same fields they are never reused, so each endpoint can evolve independently.

    /// <summary>Payload for creating a new book.</summary>
    public record CreateBookRequest(string Title, int PublishedYear, int AuthorId);

    /// <summary>Payload for replacing a book's mutable fields.</summary>
    public record UpdateBookRequest(string Title, int PublishedYear, int AuthorId);

    /// <summary>Author entry embedded inside <see cref="GetAllBooksResponse"/>.</summary>
    public record GetAllBooksAuthorEntry(int Id, string Name);

    /// <summary>Genre entry embedded inside <see cref="GetAllBooksResponse"/>.</summary>
    public record GetAllBooksGenreEntry(int Id, string Name);

    /// <summary>Response for GET /api/books — one item per book in the list.</summary>
    public record GetAllBooksResponse(
        int Id,
        string Title,
        int PublishedYear,
        GetAllBooksAuthorEntry? Author,
        IReadOnlyList<GetAllBooksGenreEntry> Genres);

    /// <summary>Author entry embedded inside <see cref="GetBookByIdResponse"/>.</summary>
    public record GetBookByIdAuthorEntry(int Id, string Name);

    /// <summary>Genre entry embedded inside <see cref="GetBookByIdResponse"/>.</summary>
    public record GetBookByIdGenreEntry(int Id, string Name);

    /// <summary>Review entry embedded inside <see cref="GetBookByIdResponse"/>.</summary>
    public record GetBookByIdReviewEntry(int Id, string ReviewerName, int Rating, string? Comment);

    /// <summary>Response for GET /api/books/{id} — includes reviews.</summary>
    public record GetBookByIdResponse(
        int Id,
        string Title,
        int PublishedYear,
        GetBookByIdAuthorEntry? Author,
        IReadOnlyList<GetBookByIdGenreEntry> Genres,
        IReadOnlyList<GetBookByIdReviewEntry> Reviews);

    /// <summary>Response for POST /api/books — contains the newly created book.</summary>
    public record CreateBookResponse(int Id, string Title, int PublishedYear, int AuthorId);
}
