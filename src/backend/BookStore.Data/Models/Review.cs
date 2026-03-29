namespace BookStore.Data.Models;

/// <summary>
/// Represents a reader's review of a book.
/// End of the Author -> Book -> Review chain.
/// </summary>
public class Review
{
    public int Id { get; set; }

    /// <summary>Rating from 1 (lowest) to 5 (highest).</summary>
    public int Rating { get; set; }

    /// <summary>Optional text comment left by the reviewer.</summary>
    public string? Comment { get; set; }

    /// <summary>Name of the person who wrote the review.</summary>
    public required string ReviewerName { get; set; }

    // Foreign key referencing the Book being reviewed
    public int BookId { get; set; }

    // Navigation property back to the Book (Book -> Review)
    public Book? Book { get; set; }
}
