using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Book"/> instances.
/// Wraps <see cref="BookBuilder"/> and provides sensible defaults so tests
/// can create a valid <see cref="Book"/> without specifying every field.
/// The <see cref="Book.Title"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestBookBuilder
{
    private readonly BookBuilder _inner = new();

    public TestBookBuilder()
    {
        WithTitle($"Book {Guid.NewGuid()}");
        WithPublishedYear(DateTime.Now.Year);
        WithAuthorId(1);
    }

    public TestBookBuilder WithTitle(string title)
    {
        _inner.WithTitle(title);
        return this;
    }

    public TestBookBuilder WithPublishedYear(int year)
    {
        _inner.WithPublishedYear(year);
        return this;
    }

    public TestBookBuilder WithAuthorId(int authorId)
    {
        _inner.WithAuthorId(authorId);
        return this;
    }

    /// <summary>Sets <see cref="Book.PublishedYear"/> to the current calendar year.</summary>
    public TestBookBuilder WithCurrentYear() => WithPublishedYear(DateTime.Now.Year);

    public Task<Book> BuildAsync() => _inner.BuildAsync();
}
