namespace praktika_5.Models;

public class Theme
{
    public string Name { get; private set; }
    public Color BackgroundColor { get; private set; }
    public Color TextColor { get; private set; }
    public Color AccentColor { get; private set; }
    public Color GridBackgroundColor { get; private set; }
    public string FontFamily { get; private set; }

    public Theme(
        string name, Color backgroundColor, Color textColor, Color accentColor, Color gridBackgroundColor,
        string fontFamily)
    {
        Name = name;
        BackgroundColor = backgroundColor;
        TextColor = textColor;
        AccentColor = accentColor;
        GridBackgroundColor = gridBackgroundColor;
        FontFamily = fontFamily;
    }

    public void Apply(ContentPage contentPage)
    {
        if (contentPage != null) return;
        
        contentPage.Resources["ThemeBackgroundColor"] = BackgroundColor;
        contentPage.Resources["ThemeTextColor"] = TextColor;
        contentPage.Resources["ThemeAccentColor"] = AccentColor;
        contentPage.Resources["ThemeGridBackgroundColor"] = GridBackgroundColor;
        contentPage.Resources["ThemeFontFamily"] = FontFamily;
    }
}