using praktika_6.Models;

namespace praktika_6.Services;

public class MealManager
{
    private readonly DatabaseService _databaseService;
    private readonly AudioService _audioService;

    public MealManager(DatabaseService databaseService, AudioService audioService)
    {
        _databaseService = databaseService;
        _audioService = audioService;
    }

    public async Task<List<MealEntry>> GetAllMealsAsync()
    {
        return await _databaseService.GetMealsAsync();
    }

    public async Task<MealEntry> AddNewMealAsync(string foodName, double amountGrams, double totalCalories, MealType mealType, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(foodName) || amountGrams <= 0) return null;

        var entry = new MealEntry
        {
            FoodName = foodName,
            AmountGrams = amountGrams,
            TotalCalories = totalCalories,
            MealType = mealType,
            Date = date
        };

        await _databaseService.SaveMealAsync(entry);
    
        try { await _audioService.PlayClickSound(); } catch { }

        return entry;
    }

    public async Task RemoveMealAsync(MealEntry meal)
    {
        if (meal == null) return;

        await _databaseService.DeleteMealAsync(meal);

        try
        {
            await _audioService.PlayClickSound();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Could not play audio: {e.Message}");
        }
    }
}