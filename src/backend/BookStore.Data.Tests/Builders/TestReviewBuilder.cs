using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Review"/> instances.
/// Inherits <see cref="ReviewBuilder"/> and pre-populates all required fields
/// so tests can create a valid <see cref="Review"/> with zero setup.
/// The <see cref="Review.ReviewerName"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestReviewBuilder : ReviewBuilder
{
    private TestBookBuilder? _bookBuilder;
    private Book? _book;

    public TestReviewBuilder()
    {
        WithReviewerName($"Reviewer {Guid.NewGuid()}");
        WithRating(5);
        WithBookId(1);
    }

    /// <summary>Sets <see cref="Review.Rating"/> to its maximum allowed value of 5.</summary>
    public TestReviewBuilder WithMaxRating()
    {
        WithRating(5);
        return this;
    }

    /// <summary>Sets <see cref="Review.Rating"/> to its minimum allowed value of 1.</summary>
    public TestReviewBuilder WithMinRating()
    {
        WithRating(1);
        return this;
    }

    /// <summary>
    /// Sets <see cref="Review.BookId"/> and <see cref="Review.Book"/> from an
    /// already-persisted <paramref name="book"/>.
    /// </summary>
    public TestReviewBuilder WithBook(Book book)
    {
        _book = book;
        _bookBuilder = null;
        WithBookId(book.Id);
        return this;
    }

    /// <summary>
    /// Configures a related <see cref="Book"/> using a <see cref="TestBookBuilder"/>.
    /// The optional <paramref name="configure"/> action lets callers override default values.
    /// The built <see cref="Book"/> is attached to <see cref="Review.Book"/> when
    /// <see cref="BuildAsync"/> is called.
    /// </summary>
    /// <example>
    /// Default book:
    /// <code>await new TestReviewBuilder().WithBook().BuildAsync();</code>
    /// Custom book:
    /// <code>await new TestReviewBuilder().WithBook(b => b.WithTitle("Dune")).BuildAsync();</code>
    /// </example>
    public TestReviewBuilder WithBook(Action<TestBookBuilder>? configure = null)
    {
        _bookBuilder = new TestBookBuilder();
        configure?.Invoke(_bookBuilder);
        _book = null;
        return this;
    }

    public new async Task<Review> BuildAsync()
    {
        if (_bookBuilder != null)
        {
            _book = await _bookBuilder.BuildAsync();
            if (_book.Id != 0)
                WithBookId(_book.Id);
        }

        var review = await base.BuildAsync();
        review.Book = _book;
        return review;
    }
}
