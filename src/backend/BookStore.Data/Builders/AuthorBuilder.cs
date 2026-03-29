using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Author"/> instances with sensible defaults.
///
/// The Test Builder pattern lets you construct objects step-by-step
/// using a fluent API, making tests easier to read and maintain.
/// Each "With" method returns the same builder so calls can be chained.
/// </summary>
public class AuthorBuilder
{
    private int _id = 1;
    private string _name = "Default Author";
    private string? _bio;

    public AuthorBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

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
    /// Constructs and returns the <see cref="Author"/> object.
    /// </summary>
    public Author Build() => new()
    {
        Id = _id,
        Name = _name,
        Bio = _bio
    };
}
