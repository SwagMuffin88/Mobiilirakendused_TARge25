using Microsoft.Extensions.DependencyInjection;
using praktika_6;

namespace praktika_6;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        Services.LocalizationResourceManager.Instance.LoadSavedCulture();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}