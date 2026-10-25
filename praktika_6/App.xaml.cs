using Microsoft.Extensions.DependencyInjection;
using praktika_6;

namespace praktika_6;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        
        try
        {
            Services.LocalizationResourceManager.Instance.LoadSavedCulture();
            new Services.ThemeService().LoadTheme();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"START ERROR: {ex.Message} \n {ex.StackTrace}");
        }
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}