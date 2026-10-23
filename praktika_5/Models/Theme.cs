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
        var resources = contentPage.Resources;

        resources["ThemeBackgroundColor"] = BackgroundColor;
        resources["ThemeTextColor"] = TextColor;
        resources["ThemeAccentColor"] = AccentColor;
        resources["ThemeGridBackgroundColor"] = GridBackgroundColor;
        resources["ThemeFontFamily"] = FontFamily;
        
        ApplyFontRecursive(contentPage, FontFamily);
    }

    private void ApplyFontRecursive(Element element, string fontFamily)
    {
        if (element == null) return;

        if (element is Label label)
        {
            label.FontFamily = fontFamily;
        }
        else if (element is Button button)
        {
            button.FontFamily = fontFamily;
        }

        
        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                if (child is Element childElement)
                {
                    ApplyFontRecursive(childElement, fontFamily);
                }
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
        {
            ApplyFontRecursive(contentView.Content, fontFamily);
        }
        else if (element is Border border && border.Content != null)
        {
            ApplyFontRecursive(border.Content, fontFamily);
        }
    }
}