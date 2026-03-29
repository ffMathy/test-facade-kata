using BookStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

/// <summary>
/// EF Core database context for the BookStore application.
/// Registers all five entity sets and configures the composite key
/// for the BookGenre join table.
/// </summary>
public class BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
    : DbContext(options)
{
    // Each DbSet corresponds to a database table
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<BookGenre> BookGenres => Set<BookGenre>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // BookGenre uses a composite primary key (BookId + GenreId)
        // because it is a pure join table without its own surrogate key.
        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });
    }
}
