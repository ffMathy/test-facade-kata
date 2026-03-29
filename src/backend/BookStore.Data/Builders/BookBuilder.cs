using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Book"/> instances.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// Note: there is no WithId() — the primary key is assigned automatically by EF.
/// </summary>
public class BookBuilder
{
    private string? _title;
    private int _publishedYear;
    private int _authorId;

    public BookBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public BookBuilder WithPublishedYear(int year)
    {
        _publishedYear = year;
        return this;
    }

    /// <summary>Links this book to an author by foreign key.</summary>
    public BookBuilder WithAuthorId(int authorId)
    {
        _authorId = authorId;
        return this;
    }

    /// <summary>
    /// Throws <see cref="InvalidOperationException"/> if required fields are not set.
    /// </summary>
    public Task<Book> BuildAsync()
    {
        if (string.IsNullOrWhiteSpace(_title))
            throw new InvalidOperationException(
                $"{nameof(Book.Title)} is required. Call {nameof(WithTitle)}() before building.");

        if (_authorId == 0)
            throw new InvalidOperationException(
                $"{nameof(Book.AuthorId)} is required. Call {nameof(WithAuthorId)}() before building.");

        if (_publishedYear == 0)
            throw new InvalidOperationException(
                $"{nameof(Book.PublishedYear)} is required. Call {nameof(WithPublishedYear)}() before building.");

        return Task.FromResult(new Book
        {
            Title = _title,
            PublishedYear = _publishedYear,
            AuthorId = _authorId
        });
    }
}
