namespace BookStore.Data.Models;

/// <summary>
/// Represents a book written by an Author.
/// Demonstrates a two-level chain: Author -> Book -> Review.
/// Also participates in a many-to-many relationship with Genre via BookGenre.
/// </summary>
public class Book
{
    public int Id { get; set; }

    /// <summary>Title of the book.</summary>
    public required string Title { get; set; }

    /// <summary>Year the book was published.</summary>
    public int PublishedYear { get; set; }

    // Foreign key referencing the Author who wrote this book
    public int AuthorId { get; set; }

    // Navigation property back to the Author (Author -> Book)
    public Author? Author { get; set; }

    // Navigation property: a book can have many reviews (Book -> Review)
    public List<Review> Reviews { get; set; } = [];

    // Navigation property: a book can belong to many genres (Book -> BookGenre -> Genre)
    public List<BookGenre> BookGenres { get; set; } = [];
}
