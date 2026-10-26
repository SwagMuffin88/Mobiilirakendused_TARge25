using praktika_6.Models;

namespace praktika_6.Services;

public class NutritionCalculator
{
    public double CalculateMealCalories(double amountGrams, double caloriesPer100g)
    {
        var calories = (amountGrams / 100.0) * caloriesPer100g;
        return Math.Round(calories, 1);
    }
    
    public double CalculateTotalCalories(IEnumerable<MealEntry> meals)
    {
        return meals.Sum(m => m.TotalCalories);
    }
}