using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Review"/> instances.
/// Wraps <see cref="ReviewBuilder"/> and provides sensible defaults so tests
/// can create a valid <see cref="Review"/> without specifying every field.
/// The <see cref="Review.ReviewerName"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestReviewBuilder
{
    private readonly ReviewBuilder _inner = new();

    public TestReviewBuilder()
    {
        WithReviewerName($"Reviewer {Guid.NewGuid()}");
        WithRating(5);
        WithBookId(1);
    }

    public TestReviewBuilder WithReviewerName(string name)
    {
        _inner.WithReviewerName(name);
        return this;
    }

    public TestReviewBuilder WithRating(int rating)
    {
        _inner.WithRating(rating);
        return this;
    }

    public TestReviewBuilder WithComment(string comment)
    {
        _inner.WithComment(comment);
        return this;
    }

    public TestReviewBuilder WithBookId(int bookId)
    {
        _inner.WithBookId(bookId);
        return this;
    }

    /// <summary>Sets <see cref="Review.Rating"/> to its maximum allowed value of 5.</summary>
    public TestReviewBuilder WithMaxRating() => WithRating(5);

    /// <summary>Sets <see cref="Review.Rating"/> to its minimum allowed value of 1.</summary>
    public TestReviewBuilder WithMinRating() => WithRating(1);

    /// <summary>
    /// Sets <see cref="Review.BookId"/> from the <see cref="Book.Id"/> of
    /// an already-persisted <paramref name="book"/>.
    /// </summary>
    public TestReviewBuilder WithBook(Book book) => WithBookId(book.Id);

    public Task<Review> BuildAsync() => _inner.BuildAsync();
}
