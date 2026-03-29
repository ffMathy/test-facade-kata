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
    private TestGenreBuilder? _genreBuilder;

    public TestBookGenreBuilder()
    {
        WithBookId(1);
        WithGenreId(1);
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
        return this;
    }

    public new async Task<BookGenre> BuildAsync()
    {
        Book? book = null;
        if (_bookBuilder != null)
        {
            book = await _bookBuilder.BuildAsync();
            WithBookId(book.Id);
        }

        Genre? genre = null;
        if (_genreBuilder != null)
        {
            genre = await _genreBuilder.BuildAsync();
            WithGenreId(genre.Id);
        }

        var bookGenre = await base.BuildAsync();
        bookGenre.Book = book;
        bookGenre.Genre = genre;
        return bookGenre;
    }
}
