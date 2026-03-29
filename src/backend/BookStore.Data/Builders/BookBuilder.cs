using BookStore.Data.Models;

namespace BookStore.Data.Builders;

/// <summary>
/// Builder for creating <see cref="Book"/> instances with sensible defaults.
/// See <see cref="AuthorBuilder"/> for a description of the builder pattern.
/// </summary>
public class BookBuilder
{
    private int _id = 1;
    private string _title = "Default Book";
    private int _publishedYear = 2000;
    private int _authorId = 1;

    public BookBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public BookBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public BookBuilder WithPublishedYear(int year)
    {
        _publishedYear = year;
        return this;
    }

    /// <summary>Links this book to an author by foreign key.</summary>
    public BookBuilder WithAuthorId(int authorId)
    {
        _authorId = authorId;
        return this;
    }

    public Book Build() => new()
    {
        Id = _id,
        Title = _title,
        PublishedYear = _publishedYear,
        AuthorId = _authorId
    };
}
