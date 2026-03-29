using BookStore.Data;
using BookStore.Data.Builders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------
// Register services
// -----------------------------------------------------------------

// Use an in-memory database so the app runs without any setup.
// In a real application you would swap this for a SQL Server or
// PostgreSQL provider and supply a connection string.
builder.Services.AddDbContext<BookStoreDbContext>(opt =>
    opt.UseInMemoryDatabase("BookStore"));

// Register MVC controllers
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        // Prevent infinite loops when serialising circular navigation properties
        opt.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Allow the React frontend (running on a different port) to call the API.
// In production you would lock this down to specific origins.
builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// -----------------------------------------------------------------
// Seed demo data so the UI has something to show on first load
// -----------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();

    // Authors
    var tolkien = new AuthorBuilder()
        .WithId(1).WithName("J.R.R. Tolkien")
        .WithBio("English author, poet, and academic best known for The Lord of the Rings.")
        .Build();

    var orwell = new AuthorBuilder()
        .WithId(2).WithName("George Orwell")
        .WithBio("English novelist and essayist famous for 1984 and Animal Farm.")
        .Build();

    db.Authors.AddRange(tolkien, orwell);

    // Genres
    var fantasy = new GenreBuilder().WithId(1).WithName("Fantasy").Build();
    var dystopian = new GenreBuilder().WithId(2).WithName("Dystopian").Build();
    var classic = new GenreBuilder().WithId(3).WithName("Classic").Build();
    db.Genres.AddRange(fantasy, dystopian, classic);

    // Books (Author -> Book)
    var lotr = new BookBuilder()
        .WithId(1).WithTitle("The Lord of the Rings")
        .WithPublishedYear(1954).WithAuthorId(1)
        .Build();

    var hobbit = new BookBuilder()
        .WithId(2).WithTitle("The Hobbit")
        .WithPublishedYear(1937).WithAuthorId(1)
        .Build();

    var nineteenEightyFour = new BookBuilder()
        .WithId(3).WithTitle("1984")
        .WithPublishedYear(1949).WithAuthorId(2)
        .Build();

    db.Books.AddRange(lotr, hobbit, nineteenEightyFour);

    // Book -> Genre links (Book -> BookGenre -> Genre)
    db.BookGenres.AddRange(
        new BookStore.Data.Models.BookGenre { BookId = 1, GenreId = 1 },  // LotR -> Fantasy
        new BookStore.Data.Models.BookGenre { BookId = 1, GenreId = 3 },  // LotR -> Classic
        new BookStore.Data.Models.BookGenre { BookId = 2, GenreId = 1 },  // Hobbit -> Fantasy
        new BookStore.Data.Models.BookGenre { BookId = 3, GenreId = 2 },  // 1984 -> Dystopian
        new BookStore.Data.Models.BookGenre { BookId = 3, GenreId = 3 }   // 1984 -> Classic
    );

    // Reviews (Book -> Review)
    db.Reviews.AddRange(
        new ReviewBuilder()
            .WithId(1).WithBookId(1).WithRating(5)
            .WithReviewerName("Alice")
            .WithComment("An epic masterpiece!")
            .Build(),
        new ReviewBuilder()
            .WithId(2).WithBookId(1).WithRating(4)
            .WithReviewerName("Bob")
            .WithComment("Long but worth every page.")
            .Build(),
        new ReviewBuilder()
            .WithId(3).WithBookId(3).WithRating(5)
            .WithReviewerName("Carol")
            .WithComment("A chilling vision of the future.")
            .Build()
    );

    db.SaveChanges();
}

// -----------------------------------------------------------------
// Middleware pipeline
// -----------------------------------------------------------------
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
