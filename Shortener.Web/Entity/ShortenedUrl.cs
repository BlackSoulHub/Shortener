namespace Shortener.Web.Entity;

public class ShortenedUrl
{
    public int Id { get; init; }
    public required string LongUrl { get; init; }
    public required string ShortUrl { get; init; }
    public required string Code { get; init; }
    public required DateTime CreatedOnUtc { get; init; }
}