namespace BookStore.Data.Models;

/// <summary>
/// Represents an author who can write one or more books.
/// This is the "root" entity in the Author -> Book -> Review chain.
/// </summary>
public class Author
{
    public int Id { get; set; }

    /// <summary>Full name of the author.</summary>
    public required string Name { get; set; }

    /// <summary>Short biography of the author.</summary>
    public string? Bio { get; set; }

    // Navigation property: one author can have many books
    public List<Book> Books { get; set; } = [];
}
