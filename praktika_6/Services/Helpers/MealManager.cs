using praktika_6.Models;

namespace praktika_6.Services;

public class MealManager
{
    private readonly DatabaseService _databaseService;
    private readonly NutritionCalculator _calculator;
    private readonly AudioService _audioService;

    public MealManager(DatabaseService databaseService, NutritionCalculator calculator, AudioService audioService)
    {
        _databaseService = databaseService;
        _calculator = calculator;
        _audioService = audioService;
    }

    public async Task<List<MealEntry>> GetAllMealsAsync()
    {
        return await _databaseService.GetMealsAsync();
    }

    public async Task<MealEntry> AddNewMealAsync(string foodName, double amountGrams)
    {
        if (string.IsNullOrWhiteSpace(foodName)) return null;

        // TODO add API call
        double caloriesPer100g = 150; // placeholder value 
        double calculatedCalories = _calculator.CalculateMealCalories(amountGrams, caloriesPer100g);

        var entry = new MealEntry
        {
            FoodName = foodName,
            AmountGrams = amountGrams,
            TotalCalories = calculatedCalories,
            Date = DateTime.Now
        };

        await _databaseService.SaveMealAsync(entry);
        await _audioService.PlayClickSound();

        return entry;
    }
}