using BookStore.Data.Builders;
using BookStore.Data.Models;

namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Test builder for <see cref="Author"/> instances.
/// Inherits <see cref="AuthorBuilder"/> and pre-populates all required fields
/// so tests can create a valid <see cref="Author"/> with zero setup.
/// The <see cref="Author.Name"/> is unique by default (GUID interpolation)
/// so multiple calls in the same test will never collide.
/// </summary>
public class TestAuthorBuilder : AuthorBuilder
{
    public TestAuthorBuilder()
    {
        WithName($"Author {Guid.NewGuid()}");
    }

    public new async Task<Author> BuildAsync()
    {
        var author = await base.BuildAsync();
        author.Id = TestId.New();
        return author;
    }
}
