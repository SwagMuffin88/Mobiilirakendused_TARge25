using Microsoft.Extensions.DependencyInjection;

namespace praktika_3;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var tabbedPage = new TabbedPage();
		tabbedPage.Children.Add(new GamePage { Title = "Mäng" });
		tabbedPage.Children.Add(new ScorePage());
		
		return new Window(tabbedPage);
	}
}