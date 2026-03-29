using BookStore.Data;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Controllers;

/// <summary>
/// Endpoints for managing Reviews on a specific Book.
/// Reviews are nested under /api/books/{bookId}/reviews to reflect
/// the Book -> Review ownership relationship.
/// </summary>
[ApiController]
[Route("api/books/{bookId}/reviews")]
public class ReviewsController(BookStoreDbContext db) : ControllerBase
{
    // GET /api/books/{bookId}/reviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetAll(int bookId)
    {
        // Verify the book exists before listing its reviews
        if (!await db.Books.AnyAsync(b => b.Id == bookId))
            return NotFound($"Book {bookId} not found.");

        return await db.Reviews
            .Where(r => r.BookId == bookId)
            .ToListAsync();
    }

    // GET /api/books/{bookId}/reviews/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetById(int bookId, int id)
    {
        var review = await db.Reviews
            .FirstOrDefaultAsync(r => r.Id == id && r.BookId == bookId);

        return review is null ? NotFound() : Ok(review);
    }

    // POST /api/books/{bookId}/reviews
    [HttpPost]
    public async Task<ActionResult<Review>> Create(int bookId, Review review)
    {
        if (!await db.Books.AnyAsync(b => b.Id == bookId))
            return NotFound($"Book {bookId} not found.");

        // Always use the bookId from the route — ignore whatever was in the body
        review.BookId = bookId;
        db.Reviews.Add(review);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { bookId, id = review.Id }, review);
    }

    // PUT /api/books/{bookId}/reviews/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int bookId, int id, Review review)
    {
        if (id != review.Id || bookId != review.BookId) return BadRequest();

        db.Entry(review).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/books/{bookId}/reviews/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int bookId, int id)
    {
        var review = await db.Reviews
            .FirstOrDefaultAsync(r => r.Id == id && r.BookId == bookId);

        if (review is null) return NotFound();

        db.Reviews.Remove(review);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
