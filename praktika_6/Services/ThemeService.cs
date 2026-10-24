namespace praktika_6.Services;

public class ThemeService
{
    public void SetTheme(AppTheme theme)
    {
        Application.Current.UserAppTheme = theme;
        Preferences.Set("theme", theme.ToString());
    }

    public void LoadTheme()
    {
        var savedTheme = Preferences.Get("AppTheme", "Unspecified");

        if (Enum.TryParse<AppTheme>(savedTheme, out var theme))
        {
            Application.Current.UserAppTheme = theme;
        }
    }
}