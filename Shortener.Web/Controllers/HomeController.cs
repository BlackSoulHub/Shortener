using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortener.Web.Entity;
using Shortener.Web.Models;
using Shortener.Web.Models.Home;
using Shortener.Web.Requests;
using Shortener.Web.Services;

namespace Shortener.Web.Controllers;

public class HomeController(UrlShortenerService service, ApplicationDbContext dbContext) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm] CreateShortUrlRequest payload)
    {
        if (!Uri.TryCreate(payload.Url, UriKind.Absolute, out _))
        {
            return BadRequest("Некоректный URL");
        }

        var code = await service.CreateCodeAsync();
        var shortenedUrl = new ShortenedUrl
        {
            LongUrl = payload.Url,
            ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/click/{code}",
            Code = code,
            CreatedOnUtc = DateTime.UtcNow
        };

        await dbContext.ShortenedUrls.AddAsync(shortenedUrl);
        await dbContext.SaveChangesAsync();

        return RedirectToAction("PreviewUrl", new { code });
    }

    [HttpGet("preview")]
    public async Task<IActionResult> PreviewUrl([FromQuery] string code)
    {
        var foundedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(u => u.Code == code);
        if (foundedUrl is null)
        {
            return NotFound("Запрашиваемый URL не найден");
        }

        return View(new PreviewUrlModel
        {
            Id = foundedUrl.Id,
            Code = foundedUrl.Code,
            ShortUrl = foundedUrl.ShortUrl,
            LongUrl = foundedUrl.LongUrl,
            CreateDate = foundedUrl.CreatedOnUtc
        });
    }

    [HttpGet("click/{code}")]
    public async Task<IActionResult> DerefShortUrl([FromRoute] string code)
    {
        var foundedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(u => u.Code == code);
        if (foundedUrl is null)
        {
            return RedirectToAction("Index");
        }

        return Redirect(foundedUrl.LongUrl);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}