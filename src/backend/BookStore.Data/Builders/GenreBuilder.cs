using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Genre"/> instances with sensible defaults.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// </summary>
public class GenreBuilder
{
    private int _id = 1;
    private string _name = "Default Genre";

    public GenreBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public GenreBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public Genre Build() => new()
    {
        Id = _id,
        Name = _name
    };
}
