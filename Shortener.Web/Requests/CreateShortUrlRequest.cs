namespace Shortener.Web.Requests;

public class CreateShortUrlRequest
{
    public required string Url { get; init; }
}