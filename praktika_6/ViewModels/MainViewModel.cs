using praktika_6.Models;
using praktika_6.Services; 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using praktika_6.Resources.Localization;

namespace praktika_6.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MealManager _mealManager;
    private readonly NutritionCalculator _calculator;
    private readonly NutritionApiService _apiService;
    private readonly ThemeService _themeService;

    // ******* OBSERVABLE PROPERTIES *********
    [ObservableProperty]
    private DateTime _selectedDate = DateTime.Today;

    [ObservableProperty]
    private double _totalDailyCalories;

    [ObservableProperty] private string _breakfastFood;
    [ObservableProperty] private double _breakfastAmount;
    public ObservableCollection<MealEntry> BreakfastMeals { get; set; } = new();

    [ObservableProperty] private string _lunchFood;
    [ObservableProperty] private double _lunchAmount;
    public ObservableCollection<MealEntry> LunchMeals { get; set; } = new();

    [ObservableProperty] private string _dinnerFood;
    [ObservableProperty] private double _dinnerAmount;
    public ObservableCollection<MealEntry> DinnerMeals { get; set; } = new();
    // ********************************
    
    public MainViewModel(MealManager mealManager, NutritionCalculator calculator, NutritionApiService apiService,
        ThemeService themeService)
    {
        _mealManager = mealManager;
        _calculator = calculator;
        _apiService = apiService;
        _themeService = themeService;
        
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
    private async Task ChangeLanguageAsync()
    {
        string action = await Shell.Current.DisplayActionSheet(
            "Vali keel / Select language", 
            "Tühista / Cancel", 
            null, 
            "Eesti", 
            "English");

        if (action == "Eesti")
        {
            LocalizationResourceManager.Instance.SetCulture(new CultureInfo("et"));
            RestartAppToApplyLanguage();
        }
        else if (action == "English")
        {
            LocalizationResourceManager.Instance.SetCulture(new CultureInfo("en"));
            RestartAppToApplyLanguage();
        }
    }
    
    [RelayCommand]
    private async Task ChangeThemeAsync()
    {
        string title = AppResources.ThemeDialogTitle;
        string cancel = AppResources.Cancel;
        string light = AppResources.ThemeLight;
        string dark = AppResources.ThemeDark;
        
        string action = await Shell.Current.DisplayActionSheet(
            title, cancel, null, light, dark);

        if (action == light)
        {
            _themeService.SetTheme(AppTheme.Light);
        }
        else if (action == dark)
        {
            _themeService.SetTheme(AppTheme.Dark);
        }
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

        double apiCalorieData = await _apiService.GetCaloriesForFoodPer100GramsAsync(foodName);
        double calculatedCalories = _calculator.CalculateMealCalories(amount, apiCalorieData);
            
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
        
        string title = AppResources.DeleteConfirmTitle;
        string message = string.Format(AppResources.DeleteConfirmMessage, meal.FoodName);
        string accept = AppResources.Yes;
        string cancel = AppResources.No;
        
        bool answer = await Shell.Current.DisplayAlertAsync(title, message, accept, cancel);

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

    private void RestartAppToApplyLanguage()
    {
        Application.Current.MainPage = new AppShell();
    }
}