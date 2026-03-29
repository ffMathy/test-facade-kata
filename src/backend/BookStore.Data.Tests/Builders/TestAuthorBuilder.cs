using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Author"/> instances.
/// Wraps <see cref="AuthorBuilder"/> and provides sensible defaults so tests
/// can create a valid <see cref="Author"/> without specifying every field.
/// The <see cref="Author.Name"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestAuthorBuilder
{
    private readonly AuthorBuilder _inner = new();

    public TestAuthorBuilder()
    {
        WithName($"Author {Guid.NewGuid()}");
    }

    public TestAuthorBuilder WithName(string name)
    {
        _inner.WithName(name);
        return this;
    }

    public TestAuthorBuilder WithBio(string bio)
    {
        _inner.WithBio(bio);
        return this;
    }

    public Task<Author> BuildAsync() => _inner.BuildAsync();
}
