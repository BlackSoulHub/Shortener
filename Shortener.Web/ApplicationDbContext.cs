using Microsoft.EntityFrameworkCore;
using Shortener.Web.Entity;
using Shortener.Web.Services;

namespace Shortener.Web;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{ 
    public required DbSet<ShortenedUrl> ShortenedUrls { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(p => p.Code).HasMaxLength(UrlShortenerService.ShortUrlCharacterCount);
            builder.HasIndex(p => p.Code).IsUnique();
        });
    }
}