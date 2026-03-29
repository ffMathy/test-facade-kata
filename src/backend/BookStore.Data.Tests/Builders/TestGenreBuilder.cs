using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Genre"/> instances.
/// Wraps <see cref="GenreBuilder"/> and provides sensible defaults so tests
/// can create a valid <see cref="Genre"/> without specifying every field.
/// The <see cref="Genre.Name"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestGenreBuilder
{
    private readonly GenreBuilder _inner = new();

    public TestGenreBuilder()
    {
        WithName($"Genre {Guid.NewGuid()}");
    }

    public TestGenreBuilder WithName(string name)
    {
        _inner.WithName(name);
        return this;
    }

    public Task<Genre> BuildAsync() => _inner.BuildAsync();
}
