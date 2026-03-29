using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Book"/> instances.
/// Inherits <see cref="BookBuilder"/> and pre-populates all required fields
/// so tests can create a valid <see cref="Book"/> with zero setup.
/// The <see cref="Book.Title"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestBookBuilder : BookBuilder
{
    private TestAuthorBuilder? _authorBuilder;

    public TestBookBuilder()
    {
        WithTitle($"Book {Guid.NewGuid()}");
        WithPublishedYear(DateTime.Now.Year);
        WithAuthorId(1);
    }

    /// <summary>Sets <see cref="Book.PublishedYear"/> to the current calendar year.</summary>
    public TestBookBuilder WithCurrentYear()
    {
        WithPublishedYear(DateTime.Now.Year);
        return this;
    }

    /// <summary>
    /// Configures a related <see cref="Author"/> using a <see cref="TestAuthorBuilder"/>.
    /// The optional <paramref name="configure"/> action lets callers override default values.
    /// The built <see cref="Author"/> is attached to <see cref="Book.Author"/> when
    /// <see cref="BuildAsync"/> is called.
    /// </summary>
    /// <example>
    /// Default author:
    /// <code>await new TestBookBuilder().WithAuthor().BuildAsync();</code>
    /// Custom author:
    /// <code>await new TestBookBuilder().WithAuthor(a => a.WithName("Tolkien")).BuildAsync();</code>
    /// </example>
    public TestBookBuilder WithAuthor(Action<TestAuthorBuilder>? configure = null)
    {
        _authorBuilder = new TestAuthorBuilder();
        configure?.Invoke(_authorBuilder);
        return this;
    }

    public new async Task<Book> BuildAsync()
    {
        var book = await base.BuildAsync();
        book.Id = TestId.New();
        if (_authorBuilder != null)
        {
            var author = await _authorBuilder.BuildAsync();
            book.AuthorId = author.Id;
            book.Author = author;
        }
        return book;
    }
}
