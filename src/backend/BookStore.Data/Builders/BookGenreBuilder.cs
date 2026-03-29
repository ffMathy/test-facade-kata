using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="BookGenre"/> join entities.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// </summary>
public class BookGenreBuilder
{
    private int _bookId = 1;
    private int _genreId = 1;

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

    public BookGenre Build() => new()
    {
        BookId = _bookId,
        GenreId = _genreId
    };
}
