using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Review"/> instances.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// Note: there is no WithId() — the primary key is assigned automatically by EF.
/// </summary>
public class ReviewBuilder
{
    private int _rating;
    private string? _comment;
    private string? _reviewerName;
    private int _bookId;

    /// <summary>Rating must be between 1 and 5.</summary>
    public ReviewBuilder WithRating(int rating)
    {
        _rating = rating;
        return this;
    }

    public ReviewBuilder WithComment(string comment)
    {
        _comment = comment;
        return this;
    }

    public ReviewBuilder WithReviewerName(string name)
    {
        _reviewerName = name;
        return this;
    }

    /// <summary>Links this review to a book by foreign key.</summary>
    public ReviewBuilder WithBookId(int bookId)
    {
        _bookId = bookId;
        return this;
    }

    /// <summary>
    /// Throws <see cref="InvalidOperationException"/> if required fields are not set or invalid.
    /// </summary>
    public Task<Review> BuildAsync()
    {
        if (string.IsNullOrWhiteSpace(_reviewerName))
            throw new InvalidOperationException(
                $"{nameof(Review.ReviewerName)} is required. Call {nameof(WithReviewerName)}() before building.");

        if (_bookId == 0)
            throw new InvalidOperationException(
                $"{nameof(Review.BookId)} is required. Call {nameof(WithBookId)}() before building.");

        if (_rating < 1 || _rating > 5)
            throw new InvalidOperationException(
                $"{nameof(Review.Rating)} must be between 1 and 5. Call {nameof(WithRating)}() before building.");

        return Task.FromResult(new Review
        {
            Rating = _rating,
            Comment = _comment,
            ReviewerName = _reviewerName,
            BookId = _bookId
        });
    }
}
