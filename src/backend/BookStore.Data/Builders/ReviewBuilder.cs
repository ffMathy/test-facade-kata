using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Review"/> instances with sensible defaults.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// </summary>
public class ReviewBuilder
{
    private int _id = 1;
    private int _rating = 5;
    private string? _comment;
    private string _reviewerName = "Anonymous";
    private int _bookId = 1;

    public ReviewBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

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

    public Review Build() => new()
    {
        Id = _id,
        Rating = _rating,
        Comment = _comment,
        ReviewerName = _reviewerName,
        BookId = _bookId
    };
}
