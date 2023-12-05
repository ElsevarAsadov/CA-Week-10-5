using Microsoft.EntityFrameworkCore;
using Pustok.Models;

namespace Pustok.DAL;

public class PustokContext : DbContext
{
    public PustokContext(DbContextOptions<PustokContext> options) : base(options) { }

    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BookTag> BookTags { get; set; }
    public DbSet<BookImage> BookImages { get; set; }
}
