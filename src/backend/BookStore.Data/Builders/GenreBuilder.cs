using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Genre"/> instances.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// Note: there is no WithId() — the primary key is assigned automatically by EF.
/// </summary>
public class GenreBuilder
{
    private string? _name;

    public GenreBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    /// <summary>
    /// Throws <see cref="InvalidOperationException"/> if required fields are not set.
    /// </summary>
    public Task<Genre> BuildAsync()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException(
                $"{nameof(Genre.Name)} is required. Call {nameof(WithName)}() before building.");

        return Task.FromResult(new Genre
        {
            Name = _name
        });
    }
}
