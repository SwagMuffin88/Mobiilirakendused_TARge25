using Microsoft.Extensions.DependencyInjection;

namespace ex_2;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var startPage = new StartPage();
		var navPage = new NavigationPage(startPage)
		{
			BarBackground = Colors.Azure,
			BarTextColor = Colors.Brown
		};
		
		return new Window(navPage);
	}
}