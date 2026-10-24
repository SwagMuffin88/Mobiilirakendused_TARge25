using Microsoft.Extensions.Logging;
using praktika_6.Services;
using praktika_6.ViewModels;

namespace praktika_6;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<AudioService>();
        builder.Services.AddSingleton<NutritionCalculator>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
