using SQLite;

namespace praktika_6.Models;

public class FoodItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public double CaloriesPer100g { get; set; }
}

public class MealEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public MealType MealType { get; set; }
    public string FoodName { get; set; } = string.Empty;
    public double AmountGrams { get; set; }
    public double TotalCalories { get; set; }
    public DateTime Date { get; set; }
}

public enum MealType
{
    Breakfast,
    Lunch,
    Dinner
}