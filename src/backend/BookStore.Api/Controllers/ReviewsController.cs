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
    public async Task<ActionResult<IEnumerable<GetAllReviewsResponse>>> GetAll(int bookId)
    {
        // Verify the book exists before listing its reviews
        if (!await db.Books.AnyAsync(b => b.Id == bookId))
            return NotFound($"Book {bookId} not found.");

        var reviews = (await db.Reviews
            .Where(r => r.BookId == bookId)
            .ToListAsync())
            .Select(r => new GetAllReviewsResponse(r.Id, r.BookId, r.ReviewerName, r.Rating, r.Comment));

        return Ok(reviews);
    }

    // GET /api/books/{bookId}/reviews/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<GetReviewByIdResponse>> GetById(int bookId, int id)
    {
        var review = await db.Reviews
            .FirstOrDefaultAsync(r => r.Id == id && r.BookId == bookId);

        return review is null
            ? NotFound()
            : Ok(new GetReviewByIdResponse(review.Id, review.BookId, review.ReviewerName, review.Rating, review.Comment));
    }

    // POST /api/books/{bookId}/reviews
    [HttpPost]
    public async Task<ActionResult<CreateReviewResponse>> Create(int bookId, CreateReviewRequest request)
    {
        if (!await db.Books.AnyAsync(b => b.Id == bookId))
            return NotFound($"Book {bookId} not found.");

        var review = new Review
        {
            ReviewerName = request.ReviewerName,
            Rating = request.Rating,
            Comment = request.Comment,
            // Always use the bookId from the route — ignore whatever was in the body
            BookId = bookId
        };
        await db.Reviews.AddAsync(review);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { bookId, id = review.Id },
            new CreateReviewResponse(review.Id, review.BookId, review.ReviewerName, review.Rating, review.Comment));
    }

    // PUT /api/books/{bookId}/reviews/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int bookId, int id, UpdateReviewRequest request)
    {
        var existing = await db.Reviews.FirstOrDefaultAsync(r => r.Id == id && r.BookId == bookId);
        if (existing is null) return NotFound();

        existing.Rating = request.Rating;
        existing.Comment = request.Comment;
        existing.ReviewerName = request.ReviewerName;

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

    // ── Request / Response records ──────────────────────────────────────────────
    // Each endpoint has its own dedicated type — even when two types share the
    // same fields they are never reused, so each endpoint can evolve independently.

    /// <summary>Payload for creating a new review.</summary>
    public record CreateReviewRequest(string ReviewerName, int Rating, string? Comment);

    /// <summary>Payload for replacing a review's mutable fields.</summary>
    public record UpdateReviewRequest(string ReviewerName, int Rating, string? Comment);

    /// <summary>Response for GET /api/books/{bookId}/reviews — one item per review in the list.</summary>
    public record GetAllReviewsResponse(int Id, int BookId, string ReviewerName, int Rating, string? Comment);

    /// <summary>Response for GET /api/books/{bookId}/reviews/{id}.</summary>
    public record GetReviewByIdResponse(int Id, int BookId, string ReviewerName, int Rating, string? Comment);

    /// <summary>Response for POST /api/books/{bookId}/reviews — contains the newly created review.</summary>
    public record CreateReviewResponse(int Id, int BookId, string ReviewerName, int Rating, string? Comment);
}
