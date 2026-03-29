using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="BookGenre"/> join entities.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// </summary>
public class BookGenreBuilder
{
    private int _bookId;
    private int _genreId;

    public BookGenreBuilder WithBookId(int bookId)
    {
        _bookId = bookId;
        return this;
    }

    public BookGenreBuilder WithGenreId(int genreId)
    {
        _genreId = genreId;
        return this;
    }

    /// <summary>
    /// Throws <see cref="InvalidOperationException"/> if required fields are not set.
    /// </summary>
    public Task<BookGenre> BuildAsync()
    {
        if (_bookId == 0)
            throw new InvalidOperationException(
                $"{nameof(BookGenre.BookId)} is required. Call {nameof(WithBookId)}() before building.");

        if (_genreId == 0)
            throw new InvalidOperationException(
                $"{nameof(BookGenre.GenreId)} is required. Call {nameof(WithGenreId)}() before building.");

        return Task.FromResult(new BookGenre
        {
            BookId = _bookId,
            GenreId = _genreId
        });
    }
}
