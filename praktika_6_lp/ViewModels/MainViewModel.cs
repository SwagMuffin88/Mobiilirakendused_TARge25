using praktika_6.Models;
using praktika_6.Services; 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace praktika_6.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MealManager _mealManager;
    private readonly NutritionCalculator _calculator;

    [ObservableProperty]
    private string foodName;

    [ObservableProperty]
    private double amountGrams;

    [ObservableProperty]
    private double totalDailyCalories;

    public ObservableCollection<MealEntry> Meals { get; set; } = new();

    public MainViewModel(MealManager mealManager, NutritionCalculator calculator)
    {
        _mealManager = mealManager;
        _calculator = calculator;
        
        LoadDataCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var list = await _mealManager.GetAllMealsAsync();
        Meals.Clear();
        foreach (var item in list) Meals.Add(item);
        
        TotalDailyCalories = _calculator.CalculateTotalCalories(Meals);
    }

    [RelayCommand]
    private async Task AddMealAsync()
    {
        var newEntry = await _mealManager.AddNewMealAsync(FoodName, AmountGrams);
        if (newEntry != null)
        {
            Meals.Add(newEntry);
            TotalDailyCalories = _calculator.CalculateTotalCalories(Meals);
            
            FoodName = string.Empty;
            AmountGrams = 0;
        }
    }
}