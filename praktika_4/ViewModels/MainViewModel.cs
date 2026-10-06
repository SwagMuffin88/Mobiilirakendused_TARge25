using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using praktika_4.Models;
using praktika_4.Resources;

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
        LoadDishes();

        SwitchLanguageCommand = new Command(SwitchLanguage);
    }

    public void LoadDishes()
    {
        Dishes.Clear();
        // todo add logic for reading info from csv file
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