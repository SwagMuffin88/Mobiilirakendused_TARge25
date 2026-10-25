using System.ComponentModel;
using System.Globalization;
using praktika_6.Resources.Localization;

namespace praktika_6.Services;

public class LocalizationResourceManager : INotifyPropertyChanged
{
    public static LocalizationResourceManager Instance { get; } = new();

    public event PropertyChangedEventHandler PropertyChanged;

    public void SetCulture(CultureInfo culture)
    {
        AppResources.Culture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        
        Preferences.Set("SelectedLanguage", culture.TwoLetterISOLanguageName);
    }
    
    public void LoadSavedCulture()
    {
        string languageCode = Preferences.Get("SelectedLanguage", "et");
        SetCulture(new CultureInfo(languageCode));
    }
}