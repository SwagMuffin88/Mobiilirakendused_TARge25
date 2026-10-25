using praktika_6.Models;
using praktika_6.ViewModels;

namespace praktika_6.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}
