using System.Net;
using Gamesa.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GamesaWeb.Pages;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;

    [BindProperty(Name = "id", SupportsGet = true)]
    public string? GameId { get; set; }
    [BindProperty(SupportsGet = true)]
    public int? Count { get; set; }

    public IndexModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> OnGet()
    {
        if (string.IsNullOrEmpty(GameId))
        {
            GameId = Helpers.GenerateRandomGameId();
            return RedirectToPage(new { id = GameId });
        }
        using (var response = await _httpClient.PostAsJsonAsync($"{Request.Scheme}://{Request.Host}/initialize", new InitializeGameData(GameId, Count ?? Config.MaxPoints)))
        {
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        return Page();
    }
}