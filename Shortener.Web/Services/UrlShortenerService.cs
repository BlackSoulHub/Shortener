using Microsoft.EntityFrameworkCore;
using Shortener.Web.Entity;

namespace Shortener.Web.Services;

public class UrlShortenerService(ApplicationDbContext dbContext)
{
    public const int ShortUrlCharacterCount = 6;
    private const string Alphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";

    private readonly Random _random = new();

    public async Task<string> CreateCodeAsync()
    {
        while (true)
        {
            var generatedCode = GenerateUniqueCodeAsync();
            if (!await dbContext.ShortenedUrls.AnyAsync(s => s.Code == generatedCode))
            {
                return generatedCode;
            }
        }
    }

    private string GenerateUniqueCodeAsync()
    {
        var chars = new char[ShortUrlCharacterCount];

        for (int i = 0; i < ShortUrlCharacterCount; i++)
        {
            var randomIndex = _random.Next(Alphabet.Length - 1);
            chars[i] = Alphabet[randomIndex];
        }

        return new string(chars);
    }
    
}