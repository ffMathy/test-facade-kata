using BookStore.Data;
using BookStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Controllers;

/// <summary>
/// CRUD endpoints for Authors.
/// Authors are the root entity: Author -> Book -> Review.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController(BookStoreDbContext db) : ControllerBase
{
    // GET /api/authors
    [HttpGet]
    public async Task<IEnumerable<Author>> GetAll() =>
        await db.Authors.Include(a => a.Books).ToListAsync();

    // GET /api/authors/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetById(int id)
    {
        var author = await db.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

        return author is null ? NotFound() : Ok(author);
    }

    // POST /api/authors
    [HttpPost]
    public async Task<ActionResult<Author>> Create(Author author)
    {
        db.Authors.Add(author);
        await db.SaveChangesAsync();
        // Returns 201 Created with a Location header pointing to the new resource
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    // PUT /api/authors/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Author author)
    {
        if (id != author.Id) return BadRequest();

        db.Entry(author).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/authors/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await db.Authors.FindAsync(id);
        if (author is null) return NotFound();

        db.Authors.Remove(author);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
