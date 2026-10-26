using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace praktika_6.Services;

public class NutritionApiService
{
    private readonly HttpClient _httpClient = new();
    private string _apiKey;
    
    public NutritionApiService()
    {
        _ = LoadApiKeyAsync();
    }
    
    private async Task LoadApiKeyAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
            using var reader = new StreamReader(stream);
            string jsonContent = await reader.ReadToEndAsync();

            var config = JsonSerializer.Deserialize<AppSettings>(jsonContent);
            _apiKey = config?.UsdApiKey ?? string.Empty;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Could not read API key from appsettings.json: {ex.Message}");
        }
    }

    public async Task<double> GetCaloriesForFoodPer100GramsAsync(string foodQuery)
    {
        if (string.IsNullOrWhiteSpace(foodQuery)) return 0;

        if (string.IsNullOrEmpty(_apiKey))
        {
            await LoadApiKeyAsync();
        }
        
        try
        {
            string url = 
                $"https://api.nal.usda.gov/fdc/v1/foods/search?api_key={_apiKey}&query={Uri.EscapeDataString(foodQuery)}&pageSize=1";
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine($"USDA API error: {(int)response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"{url}");
               // return EstimateFallbackCalories(amountGrams); // Varuväärtus
            }

            var result = await response.Content.ReadFromJsonAsync<UsdaSearchResponse>();
            
            if (result?.Foods == null || result.Foods.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("USDA could not find any food items.");
                //return EstimateFallbackCalories(amountGrams);
            }

            var food = result.Foods[0];
            
            var calorieNutrient = food.FoodNutrients.FirstOrDefault(
                n => n.NutrientId == 1008 || 
                     (n.UnitName != null && n.UnitName.Equals("KCAL", StringComparison.OrdinalIgnoreCase)) ||
                     (n.NutrientName != null && n.NutrientName.Contains("kcal", StringComparison.OrdinalIgnoreCase))
            );
            
            if (calorieNutrient != null)
            {
                double caloriesPer100g = calorieNutrient.Value;
                System.Diagnostics.Debug.WriteLine($"Food found: {food.Description}" );
                System.Diagnostics.Debug.WriteLine($"Unit: {calorieNutrient.UnitName}" );
                System.Diagnostics.Debug.WriteLine($"Calories per 100g: {caloriesPer100g}");

                return caloriesPer100g;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Query exception: {ex.Message}");
        }

        return 0;

        // return EstimateFallbackCalories(amountGrams);
    }

    // private double EstimateFallbackCalories(double grams)
    // {
    //     return grams * 1.5;
    // }
}

public class UsdaSearchResponse
{
    [JsonPropertyName("foods")]
    public List<UsdaFood> Foods { get; set; }
}

public class UsdaFood
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("foodNutrients")]
    public List<UsdaNutrient> FoodNutrients { get; set; }
}

public class UsdaNutrient
{
    [JsonPropertyName("nutrientId")]
    public int NutrientId { get; set; }

    [JsonPropertyName("nutrientName")]
    public string NutrientName { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }
    
    [JsonPropertyName("unitName")]
    public string UnitName { get; set; }
}