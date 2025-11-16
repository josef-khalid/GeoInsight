using GeoInsight.Models;
using GeoInsight.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly IGeoInsightService _geo;

    public IndexModel(IGeoInsightService geo) => _geo = geo;

    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    public GeoInsightViewModel? Result { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Try to get Query from multiple sources
        var queryValue = Query?.Trim();
        
        if (string.IsNullOrWhiteSpace(queryValue))
        {
            // Try form collection with different possible names
            if (Request.Form.ContainsKey("Query"))
            {
                queryValue = Request.Form["Query"].ToString().Trim();
            }
            else if (Request.Form.ContainsKey("Input.Query"))
            {
                queryValue = Request.Form["Input.Query"].ToString().Trim();
            }
        }

        if (string.IsNullOrWhiteSpace(queryValue))
        {
            ModelState.AddModelError(nameof(Query), "Skriv en stad eller ett land.");
            return Page();
        }

        // Clear ModelState and set Query
        ModelState.Clear();
        Query = queryValue;

        try
        {
            Result = await _geo.BuildAsync(queryValue);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Ett fel uppstod: {ex.Message}");
            Result = new GeoInsightViewModel { Query = queryValue, Error = $"Ett fel uppstod: {ex.Message}" };
        }

        return Page();
    }
}
