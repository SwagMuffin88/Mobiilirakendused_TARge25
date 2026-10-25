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
    private readonly NutritionApiService _apiService;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private double totalDailyCalories;

    [ObservableProperty] private string breakfastFood;
    [ObservableProperty] private double breakfastAmount;
    public ObservableCollection<MealEntry> BreakfastMeals { get; set; } = new();

    [ObservableProperty] private string lunchFood;
    [ObservableProperty] private double lunchAmount;
    public ObservableCollection<MealEntry> LunchMeals { get; set; } = new();

    [ObservableProperty] private string dinnerFood;
    [ObservableProperty] private double dinnerAmount;
    public ObservableCollection<MealEntry> DinnerMeals { get; set; } = new();

    public MainViewModel(MealManager mealManager, NutritionCalculator calculator, NutritionApiService apiService)
    {
        _mealManager = mealManager;
        _calculator = calculator;
        _apiService = apiService;
        
        _ = InitializeDataAsync();
    }
    
    private async Task InitializeDataAsync()
    {
        try
        {
            await Task.Delay(50); 
            await LoadDataForSelectedDateAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing app data: {ex.Message}");
        }
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        _ = LoadDataForSelectedDateAsync();
    }

    [RelayCommand]
    private void PreviousDay() => SelectedDate = SelectedDate.AddDays(-1);

    [RelayCommand]
    private void NextDay() => SelectedDate = SelectedDate.AddDays(1);

    private async Task LoadDataForSelectedDateAsync()
    {
        try
        {
            var allMeals = await _mealManager.GetAllMealsAsync();

            var dayMeals = allMeals.Where(m => m.Date.Date == SelectedDate.Date).ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateMealCollection(dayMeals);
            });
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"An error occured: {e.Message}");
        }
    }

    [RelayCommand]
    private async Task AddMealAsync(MealType mealType)
    {
        (string foodName, double amount) = mealType switch
        {
            MealType.Breakfast => (BreakfastFood, BreakfastAmount),
            MealType.Lunch     => (LunchFood, LunchAmount),
            MealType.Dinner    => (DinnerFood, DinnerAmount),
            _                  => (string.Empty, 0)
        };

        if (string.IsNullOrWhiteSpace(foodName) || amount <= 0) return;

        double calculatedCalories = await _apiService.GetCaloriesForFoodAsync(foodName, amount);

        var newEntry = await _mealManager.AddNewMealAsync(foodName, amount, calculatedCalories, mealType, SelectedDate);

        if (newEntry != null)
        {
            if (mealType == MealType.Breakfast) { BreakfastMeals.Add(newEntry); BreakfastFood = string.Empty; BreakfastAmount = 0; }
            else if (mealType == MealType.Lunch) { LunchMeals.Add(newEntry); LunchFood = string.Empty; LunchAmount = 0; }
            else if (mealType == MealType.Dinner) { DinnerMeals.Add(newEntry); DinnerFood = string.Empty; DinnerAmount = 0; }

            UpdateTotalCalories();
        }
    }

    [RelayCommand]
    private async Task DeleteMealAsync(MealEntry meal)
    {
        if (meal == null) return;
        
        bool answer = await Shell.Current.DisplayAlertAsync(
            "Kustuta sissekanne", 
            $"Kas oled kindel, et soovid kustutada toidu '{meal.FoodName}'?", 
            "Jah", 
            "Ei");

        if (!answer) return;
        
        await _mealManager.RemoveMealAsync(meal);
        
        if (meal.MealType == MealType.Breakfast) BreakfastMeals.Remove(meal);
        else if (meal.MealType == MealType.Lunch) LunchMeals.Remove(meal);
        else if (meal.MealType == MealType.Dinner) DinnerMeals.Remove(meal);
        
        UpdateTotalCalories();
    }

    private void UpdateMealCollection(List<MealEntry> dayMeals)
    {
        BreakfastMeals.Clear();
        foreach (var m in dayMeals.Where(x => x.MealType == MealType.Breakfast)) BreakfastMeals.Add(m);
        
        LunchMeals.Clear();
        foreach (var m in dayMeals.Where(x => x.MealType == MealType.Lunch)) LunchMeals.Add(m);

        DinnerMeals.Clear();
        foreach (var m in dayMeals.Where(x => x.MealType == MealType.Dinner)) DinnerMeals.Add(m);

        UpdateTotalCalories();
    }

    private void UpdateTotalCalories()
    {
        var allCurrentMeals = BreakfastMeals.Concat(LunchMeals).Concat(DinnerMeals);
        TotalDailyCalories = _calculator.CalculateTotalCalories(allCurrentMeals);
    }
}