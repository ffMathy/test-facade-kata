using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="BookGenre"/> join entities.
/// Wraps <see cref="BookGenreBuilder"/> and provides sensible defaults so tests
/// can create a valid <see cref="BookGenre"/> without specifying every field.
/// </summary>
public class TestBookGenreBuilder
{
    private readonly BookGenreBuilder _inner = new();

    public TestBookGenreBuilder()
    {
        WithBookId(1);
        WithGenreId(1);
    }

    public TestBookGenreBuilder WithBookId(int bookId)
    {
        _inner.WithBookId(bookId);
        return this;
    }

    public TestBookGenreBuilder WithGenreId(int genreId)
    {
        _inner.WithGenreId(genreId);
        return this;
    }

    /// <summary>
    /// Sets <see cref="BookGenre.BookId"/> from the <see cref="Book.Id"/> of
    /// an already-persisted <paramref name="book"/>.
    /// </summary>
    public TestBookGenreBuilder WithBook(Book book) => WithBookId(book.Id);

    /// <summary>
    /// Sets <see cref="BookGenre.GenreId"/> from the <see cref="Genre.Id"/> of
    /// an already-persisted <paramref name="genre"/>.
    /// </summary>
    public TestBookGenreBuilder WithGenre(Genre genre) => WithGenreId(genre.Id);

    public Task<BookGenre> BuildAsync() => _inner.BuildAsync();
}
