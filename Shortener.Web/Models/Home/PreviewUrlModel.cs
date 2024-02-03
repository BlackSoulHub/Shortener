namespace Shortener.Web.Models.Home;

public class PreviewUrlModel
{
    public required int Id { get; init; }
    public required string Code { get; init; }
    public required string ShortUrl { get; init; }
    public required string LongUrl { get; init; }
    public required DateTime CreateDate { get; init; }
}