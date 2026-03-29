namespace BookStore.Data.Models;

/// <summary>
/// Represents a book genre/category (e.g. "Fiction", "Science").
/// Books and genres are linked via the BookGenre join table.
/// </summary>
public class Genre
{
    public int Id { get; set; }

    /// <summary>Name of the genre (e.g. "Fiction", "History").</summary>
    public required string Name { get; set; }

    // Navigation property: a genre can appear on many books (via BookGenre)
    public List<BookGenre> BookGenres { get; set; } = [];
}
