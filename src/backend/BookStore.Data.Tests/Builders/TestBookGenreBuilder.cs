using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="BookGenre"/> join entities.
/// Inherits <see cref="BookGenreBuilder"/> and pre-populates all required fields
/// so tests can create a valid <see cref="BookGenre"/> with zero setup.
/// </summary>
public class TestBookGenreBuilder : BookGenreBuilder
{
    private TestBookBuilder? _bookBuilder;
    private Book? _book;
    private TestGenreBuilder? _genreBuilder;
    private Genre? _genre;

    public TestBookGenreBuilder()
    {
        WithBookId(1);
        WithGenreId(1);
    }

    /// <summary>
    /// Sets <see cref="BookGenre.BookId"/> and <see cref="BookGenre.Book"/> from an
    /// already-persisted <paramref name="book"/>.
    /// </summary>
    public TestBookGenreBuilder WithBook(Book book)
    {
        _book = book;
        _bookBuilder = null;
        WithBookId(book.Id);
        return this;
    }

    /// <summary>
    /// Configures a related <see cref="Book"/> using a <see cref="TestBookBuilder"/>.
    /// The optional <paramref name="configure"/> action lets callers override default values.
    /// The built <see cref="Book"/> is attached to <see cref="BookGenre.Book"/> when
    /// <see cref="BuildAsync"/> is called.
    /// </summary>
    /// <example>
    /// Default book:
    /// <code>await new TestBookGenreBuilder().WithBook().BuildAsync();</code>
    /// Custom book:
    /// <code>await new TestBookGenreBuilder().WithBook(b => b.WithTitle("Dune")).BuildAsync();</code>
    /// </example>
    public TestBookGenreBuilder WithBook(Action<TestBookBuilder>? configure = null)
    {
        _bookBuilder = new TestBookBuilder();
        configure?.Invoke(_bookBuilder);
        _book = null;
        return this;
    }

    /// <summary>
    /// Sets <see cref="BookGenre.GenreId"/> and <see cref="BookGenre.Genre"/> from an
    /// already-persisted <paramref name="genre"/>.
    /// </summary>
    public TestBookGenreBuilder WithGenre(Genre genre)
    {
        _genre = genre;
        _genreBuilder = null;
        WithGenreId(genre.Id);
        return this;
    }

    /// <summary>
    /// Configures a related <see cref="Genre"/> using a <see cref="TestGenreBuilder"/>.
    /// The optional <paramref name="configure"/> action lets callers override default values.
    /// The built <see cref="Genre"/> is attached to <see cref="BookGenre.Genre"/> when
    /// <see cref="BuildAsync"/> is called.
    /// </summary>
    /// <example>
    /// Default genre:
    /// <code>await new TestBookGenreBuilder().WithGenre().BuildAsync();</code>
    /// Custom genre:
    /// <code>await new TestBookGenreBuilder().WithGenre(g => g.WithName("Fantasy")).BuildAsync();</code>
    /// </example>
    public TestBookGenreBuilder WithGenre(Action<TestGenreBuilder>? configure = null)
    {
        _genreBuilder = new TestGenreBuilder();
        configure?.Invoke(_genreBuilder);
        _genre = null;
        return this;
    }

    public new async Task<BookGenre> BuildAsync()
    {
        if (_bookBuilder != null)
        {
            _book = await _bookBuilder.BuildAsync();
            if (_book.Id != 0)
                WithBookId(_book.Id);
        }

        if (_genreBuilder != null)
        {
            _genre = await _genreBuilder.BuildAsync();
            if (_genre.Id != 0)
                WithGenreId(_genre.Id);
        }

        var bookGenre = await base.BuildAsync();
        bookGenre.Book = _book;
        bookGenre.Genre = _genre;
        return bookGenre;
    }
}
