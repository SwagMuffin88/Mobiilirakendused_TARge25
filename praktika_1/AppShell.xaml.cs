namespace praktika_1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(TrafficLightPage), typeof(TrafficLightPage));
	}
}
