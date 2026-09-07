using Microsoft.Extensions.DependencyInjection;

namespace example_app;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// var startPage = new StartPage();
		//
		// var navPage = new NavigationPage(startPage)
		// {
		// 	BarBackgroundColor = Colors.AliceBlue,
		// 	BarTextColor = Colors.SlateBlue
		// };
		//
		// return new Window(navPage);
		return new Window(new AppShell());
	}
}