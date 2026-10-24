using System.Net.Http.Json;

namespace praktika_6.Services;

public class NutritionApiService
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<ApiFoodResult>> SearchFoodAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2) return new();

        try
        {
            string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={Uri.EscapeDataString(query)}&search_simple=1&action=process&json=1&page_size=5";

            var response = await _httpClient.GetFromJsonAsync<OpenFoodFactsResponse>(url);

            if (response?.Products == null) return new();

            return response.Products
                .Where(p => !string.IsNullOrEmpty(p.ProductName)
                            && p.Nutriments?.EnergyKcal100g > 0)
                .Select(p => new ApiFoodResult
                {
                    Name = p.ProductName,
                    CaloriesPer100g = p.Nutriments.EnergyKcal100g
                })
                .ToList();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"An error occured while making API request: {e.Message}");
            return new();
        }
    }
}

public class ApiFoodResult
{
    public string Name { get; set; }
    public double CaloriesPer100g { get; set; }
}

public class OpenFoodFactsResponse
{
    public List<ApiProduct> Products { get; set; }
}

public class ApiProduct
{
    public string ProductName { get; set; }
    public ApiNutriments Nutriments { get; set; }
}

public class ApiNutriments
{
    [System.Text.Json.Serialization.JsonPropertyName("energy-kcal_100g")]
    public double EnergyKcal100g { get; set; }
}