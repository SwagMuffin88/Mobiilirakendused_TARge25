using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using praktika_4.Models;
using praktika_4.Resources;
using praktika_4.Resources.Localization;

namespace praktika_4.ViewModels;

public class MainViewModel
{
    private int _position;
    public int Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Dish> Dishes { get; set; } = new();
    
    public ICommand SwitchLanguageCommand { get; }

    public MainViewModel()
    {
        LoadDishesFromCsvAsync();

        SwitchLanguageCommand = new Command(SwitchLanguage);
    }

    public async Task LoadDishesFromCsvAsync()
    {
        Dishes.Clear();
        
        string lang = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        string fileName = lang == "et" ? "dishes.et.csv" : "dishes.en.csv";

        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);

            bool isHeader = true;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                var parts = line.Split('|');

                if (parts.Length >= 6)
                {
                    Dishes.Add(new Dish
                    {
                        Name = parts[0].Trim(),
                        ImageUrl = parts[1].Trim(),
                        ShortDescription = parts[2].Trim(),
                        FullDescription = parts[3].Trim(),
                        PrepTime = parts[4].Trim(),
                        Ingredients = parts[5].Trim()
                    });
                }
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Viga CSV-faili lugemisel: {e.Message}");      
        }
    }

    public void SwitchLanguage()
    {
        var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        var newCulture = currentCulture == "et" ? new CultureInfo("en") : new CultureInfo("et");
        
        Thread.CurrentThread.CurrentUICulture = newCulture;
        Thread.CurrentThread.CurrentCulture = newCulture;
        AppResources.Culture = newCulture;

        Application.Current.MainPage = new AppShell();
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}