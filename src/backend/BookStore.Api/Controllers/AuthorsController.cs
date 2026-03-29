using BookStore.Data;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Controllers;

/// <summary>
/// CRUD endpoints for Authors.
/// Authors are the root entity: Author -> Book -> Review.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController(BookStoreDbContext db) : ControllerBase
{
    // GET /api/authors
    [HttpGet]
    public async Task<IEnumerable<GetAllAuthorsResponse>> GetAll()
    {
        var authors = await db.Authors.Include(a => a.Books).ToListAsync();
        return authors.Select(a => new GetAllAuthorsResponse(
            a.Id,
            a.Name,
            a.Bio,
            a.Books.Select(b => new GetAllAuthorsBookEntry(b.Id, b.Title, b.PublishedYear)).ToList()
        ));
    }

    // GET /api/authors/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<GetAuthorByIdResponse>> GetById(int id)
    {
        var author = await db.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author is null) return NotFound();

        return Ok(new GetAuthorByIdResponse(
            author.Id,
            author.Name,
            author.Bio,
            author.Books.Select(b => new GetAuthorByIdBookEntry(b.Id, b.Title, b.PublishedYear)).ToList()
        ));
    }

    // POST /api/authors
    [HttpPost]
    public async Task<ActionResult<CreateAuthorResponse>> Create(CreateAuthorRequest request)
    {
        var author = new Author { Name = request.Name, Bio = request.Bio };
        await db.Authors.AddAsync(author);
        await db.SaveChangesAsync();
        // Returns 201 Created with a Location header pointing to the new resource
        return CreatedAtAction(nameof(GetById), new { id = author.Id },
            new CreateAuthorResponse(author.Id, author.Name, author.Bio));
    }

    // PUT /api/authors/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAuthorRequest request)
    {
        // Fetch the tracked entity first, then update only its mutable properties.
        // This is safer than attaching a detached entity with EntityState.Modified
        // because it avoids accidentally overwriting fields not included in the payload.
        var existing = await db.Authors.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Name = request.Name;
        existing.Bio = request.Bio;

        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/authors/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await db.Authors.FindAsync(id);
        if (author is null) return NotFound();

        // Remove is synchronous — the actual delete hits the DB in SaveChangesAsync
        db.Authors.Remove(author);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Request / Response records ──────────────────────────────────────────────
    // Each endpoint has its own dedicated type — even when two types share the
    // same fields they are never reused, so each endpoint can evolve independently.

    /// <summary>Payload for creating a new author.</summary>
    public record CreateAuthorRequest(string Name, string? Bio);

    /// <summary>Payload for replacing an author's mutable fields.</summary>
    public record UpdateAuthorRequest(string Name, string? Bio);

    /// <summary>Book entry embedded inside <see cref="GetAllAuthorsResponse"/>.</summary>
    public record GetAllAuthorsBookEntry(int Id, string Title, int PublishedYear);

    /// <summary>Response for GET /api/authors — one item per author in the list.</summary>
    public record GetAllAuthorsResponse(
        int Id,
        string Name,
        string? Bio,
        IReadOnlyList<GetAllAuthorsBookEntry> Books);

    /// <summary>Book entry embedded inside <see cref="GetAuthorByIdResponse"/>.</summary>
    public record GetAuthorByIdBookEntry(int Id, string Title, int PublishedYear);

    /// <summary>Response for GET /api/authors/{id}.</summary>
    public record GetAuthorByIdResponse(
        int Id,
        string Name,
        string? Bio,
        IReadOnlyList<GetAuthorByIdBookEntry> Books);

    /// <summary>Response for POST /api/authors — contains the newly created author.</summary>
    public record CreateAuthorResponse(int Id, string Name, string? Bio);
}
