using praktika_4.Models;
using praktika_4.Resources;
using praktika_4.ViewModels;

namespace praktika_4;

public partial class MainPage : ContentPage
{
    private IDispatcherTimer _timer;
    public MainPage()
    {
        InitializeComponent();
        StartAutoScroll();
    }
    
    private void StartAutoScroll()
    {
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(4);
		
        _timer.Tick += (s, e) =>
        {
            if (BindingContext is MainViewModel vm && vm.Dishes.Count > 0)
            {
                vm.Position = (vm.Position + 1) % vm.Dishes.Count;
            }
        };
        _timer.Start();
    }
	
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
    }
	
    private async void OnDishTapped(object sender, EventArgs e)
    {
        if (sender is Element element && element.BindingContext is Dish dish)
        {
            string message = $"{dish.FullDescription}\n\n" +
                             $"{AppResources.PrepTime} {dish.PrepTime}\n\n" +
                             $"{AppResources.Ingredients}\n{dish.Ingredients}";
    
            await DisplayAlert(dish.Name, message, AppResources.Close);
        }
    }
}