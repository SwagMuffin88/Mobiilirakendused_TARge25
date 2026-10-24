using praktika_6.Models;
using praktika_6.ViewModels;

namespace praktika_6;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }

    private async void OnItemTapped(object sender, EventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is MealEntry selectedMeal)
        {
            await DisplayAlert(
                "Söögikord", 
                $"Valisid toidu: {selectedMeal.FoodName} ({selectedMeal.TotalCalories} kcal)", 
                "OK"
            );
        }
    }
}
