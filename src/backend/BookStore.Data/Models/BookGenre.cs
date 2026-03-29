namespace BookStore.Data.Models;

/// <summary>
/// Join table that links Books to Genres (many-to-many relationship).
/// A single book can have multiple genres, and a genre can apply to many books.
/// Chain example: Book -> BookGenre -> Genre
/// </summary>
public class BookGenre
{
    // Foreign key to Book
    public int BookId { get; set; }

    // Navigation property back to the Book
    public Book? Book { get; set; }

    // Foreign key to Genre
    public int GenreId { get; set; }

    // Navigation property back to the Genre
    public Genre? Genre { get; set; }
}
