using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_5;

public partial class PickerImagePage : ContentPage
{
    Grid _gr4x1, _gr3x3;
    Picker _picker;
    Image _image;
    Switch _imageSwitch, _gridSwitch;
    Random random = new Random();
    
    public PickerImagePage()
    {
        _gr4x1 = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength( 1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength( 1, GridUnitType.Star) },
            },
        };

        _picker = new Picker
        {
            Title = "Vali pilt",
            ItemsSource = new List<string> { "Pilt1", "Pilt2", "Pilt3" },
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        _picker.SelectedIndexChanged += SelectImages;

        _image = new Image
        {
            Source = "dotnet_bot.png",
            HorizontalOptions = LayoutOptions.Center,
        };
        
        _imageSwitch = new Switch
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            IsToggled = true,
            IsEnabled = true
        };
        
        _imageSwitch.Toggled += (sender, e) =>
        {
            if (e.Value)
            {
                _image.IsVisible = true;
            }
            else
            {
                _image.IsVisible = false;
            }
        };

        _gridSwitch = new Switch
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            IsToggled = false,
            IsEnabled = true
        };

        _gridSwitch.Toggled += (sender, e) =>
        {
            if (e.Value)
            {
                _gr3x3 = Fill3X3Grid();
                _gr4x1.Add(_gr3x3, 0, 2);
                _gr4x1.SetColumnSpan(_gr3x3, 2);
            }
            else
            {
                _gr4x1.RemoveAt(4);
            }
        };
            
        _gr4x1.Add(_picker, 0, 0);
        _gr4x1.SetColumnSpan(_picker, 2);
        _gr4x1.Add(_image, 0, 1);
        _gr4x1.SetColumnSpan(_image, 2);
        _gr4x1.Add(_gridSwitch, 0, 3);
        _gr4x1.Add(_imageSwitch, 1, 3);
        
        Content = _gr4x1;
    }

    private void SelectImages(object? sender, EventArgs e)
    {
        if (_picker.SelectedIndex == 1) return;
        if (_picker.SelectedIndex == 0) _image.Source = "pilt1.png";
        else if (_picker.SelectedIndex == 1) _image.Source = "pilt2.png";
        else if (_picker.SelectedIndex == 2) _image.Source = "pilt3.png";
    }

    private Grid Fill3X3Grid()
    {
        _gr3x3 = new Grid();
        for (int i = 0; i < 3; i++)
        {
            _gr3x3.RowDefinitions.Add(
                new RowDefinition {  Height = new GridLength(1, GridUnitType.Star) });
            _gr3x3.ColumnDefinitions.Add(
                new ColumnDefinition {  Width = new GridLength(1, GridUnitType.Star) });
        }

        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                BoxView box = new BoxView
                {
                    BackgroundColor = Color.FromRgb(
                        random.Next(256), random.Next(256), random.Next(256)),
                };
                
                _gr3x3.Add(box, column, row);

                TapGestureRecognizer tap = new TapGestureRecognizer();

                tap.Tapped += (s, args) =>
                {
                    box.BackgroundColor = Color.FromRgb(
                        random.Next(256), random.Next(256), random.Next(256)
                    );
                };
                
                box.GestureRecognizers.Add(tap);
            }
        }

        return _gr3x3;
    }
}