using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Author"/> instances.
///
/// The Test Builder pattern lets you construct objects step-by-step
/// using a fluent API, making tests easier to read and maintain.
/// Each "With" method returns the same builder so calls can be chained.
///
/// Call <see cref="BuildAsync"/> once all required fields have been set.
/// An <see cref="InvalidOperationException"/> is thrown if any required field is missing.
///
/// Note: there is no WithId() method — the primary key is assigned automatically
/// by Entity Framework when the entity is saved to the database.
/// </summary>
public class AuthorBuilder
{
    // null signals "not yet set"; BuildAsync will throw if still null
    private string? _name;
    private string? _bio;

    public AuthorBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public AuthorBuilder WithBio(string bio)
    {
        _bio = bio;
        return this;
    }

    /// <summary>
    /// Constructs and returns the <see cref="Author"/> object asynchronously.
    /// Throws <see cref="InvalidOperationException"/> if any required field has not been set.
    /// </summary>
    public Task<Author> BuildAsync()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException(
                $"{nameof(Author.Name)} is required. Call {nameof(WithName)}() before building.");

        return Task.FromResult(new Author
        {
            Name = _name,
            Bio = _bio
        });
    }
}
