using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Genre"/> instances.
/// Inherits <see cref="GenreBuilder"/> and pre-populates all required fields
/// so tests can create a valid <see cref="Genre"/> with zero setup.
/// The <see cref="Genre.Name"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestGenreBuilder : GenreBuilder
{
    public TestGenreBuilder()
    {
        WithName($"Genre {Guid.NewGuid()}");
    }

    public new async Task<Genre> BuildAsync()
    {
        var genre = await base.BuildAsync();
        genre.Id = TestId.New();
        return genre;
    }
}
