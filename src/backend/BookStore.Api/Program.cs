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

    // ── Stage 1: Authors and Genres ───────────────────────────────────────
    // We save these first so that EF assigns their auto-generated IDs.
    // Those IDs are then available on the objects for use in stage 2.

    var tolkien = await new AuthorBuilder()
        .WithName("J.R.R. Tolkien")
        .WithBio("English author, poet, and academic best known for The Lord of the Rings.")
        .BuildAsync();

    var orwell = await new AuthorBuilder()
        .WithName("George Orwell")
        .WithBio("English novelist and essayist famous for 1984 and Animal Farm.")
        .BuildAsync();

    await db.Authors.AddRangeAsync(tolkien, orwell);

    var fantasy  = await new GenreBuilder().WithName("Fantasy").BuildAsync();
    var dystopian = await new GenreBuilder().WithName("Dystopian").BuildAsync();
    var classic  = await new GenreBuilder().WithName("Classic").BuildAsync();

    await db.Genres.AddRangeAsync(fantasy, dystopian, classic);

    // Flush to the database → tolkien.Id, orwell.Id, fantasy.Id … are now set
    await db.SaveChangesAsync();

    // ── Stage 2: Books (reference the IDs assigned in stage 1) ───────────
    var lotr = await new BookBuilder()
        .WithTitle("The Lord of the Rings")
        .WithPublishedYear(1954).WithAuthorId(tolkien.Id)
        .BuildAsync();

    var hobbit = await new BookBuilder()
        .WithTitle("The Hobbit")
        .WithPublishedYear(1937).WithAuthorId(tolkien.Id)
        .BuildAsync();

    var nineteenEightyFour = await new BookBuilder()
        .WithTitle("1984")
        .WithPublishedYear(1949).WithAuthorId(orwell.Id)
        .BuildAsync();

    await db.Books.AddRangeAsync(lotr, hobbit, nineteenEightyFour);

    // Flush → lotr.Id, hobbit.Id … are now set
    await db.SaveChangesAsync();

    // ── Stage 3: Join rows and Reviews (reference IDs from stages 1 & 2) ─
    // Book -> Genre links (Book -> BookGenre -> Genre)
    await db.BookGenres.AddRangeAsync(
        await new BookGenreBuilder().WithBookId(lotr.Id).WithGenreId(fantasy.Id).BuildAsync(),            // LotR -> Fantasy
        await new BookGenreBuilder().WithBookId(lotr.Id).WithGenreId(classic.Id).BuildAsync(),            // LotR -> Classic
        await new BookGenreBuilder().WithBookId(hobbit.Id).WithGenreId(fantasy.Id).BuildAsync(),          // Hobbit -> Fantasy
        await new BookGenreBuilder().WithBookId(nineteenEightyFour.Id).WithGenreId(dystopian.Id).BuildAsync(), // 1984 -> Dystopian
        await new BookGenreBuilder().WithBookId(nineteenEightyFour.Id).WithGenreId(classic.Id).BuildAsync()    // 1984 -> Classic
    );

    // Reviews (Book -> Review)
    await db.Reviews.AddRangeAsync(
        await new ReviewBuilder()
            .WithBookId(lotr.Id).WithRating(5)
            .WithReviewerName("Alice")
            .WithComment("An epic masterpiece!")
            .BuildAsync(),
        await new ReviewBuilder()
            .WithBookId(lotr.Id).WithRating(4)
            .WithReviewerName("Bob")
            .WithComment("Long but worth every page.")
            .BuildAsync(),
        await new ReviewBuilder()
            .WithBookId(nineteenEightyFour.Id).WithRating(5)
            .WithReviewerName("Carol")
            .WithComment("A chilling vision of the future.")
            .BuildAsync()
    );

    await db.SaveChangesAsync();
}

// -----------------------------------------------------------------
// Middleware pipeline
// -----------------------------------------------------------------
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
