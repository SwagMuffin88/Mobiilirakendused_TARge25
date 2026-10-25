namespace praktika_6.Services;

public class ThemeService
{
    private const string ThemeKey = "AppTheme";
    
    public void SetTheme(AppTheme theme)
    {
        Application.Current.UserAppTheme = theme;
        Preferences.Set(ThemeKey, theme.ToString());
    }

    public void LoadTheme()
    {
        var savedTheme = Preferences.Get(ThemeKey, "Unspecified");

        if (Enum.TryParse<AppTheme>(savedTheme, out var theme))
        {
            Application.Current.UserAppTheme = theme;
        }
    }
}