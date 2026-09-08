using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;

namespace ex_2;

public partial class FigurePage : ContentPage
{
    private BoxView _boxView;
    private Ellipse _ellipse;
    private Polygon _polygon;
    
    private Random _random = new Random();
    private HorizontalStackLayout _horizontalStackLayout;
    private VerticalStackLayout _verticalStackLayout;
    
    List<string> buttons = new List<string>()
    {
        "Back",
        "Home",
        "Next"
    };
    
    public FigurePage()
    {
        int Red = _random.Next(256);
        int Green = _random.Next(256);
        int Blue = _random.Next(256);
        
        _boxView = new BoxView
        {
            Color = Color.FromRgb(Red, Green, Blue),
            WidthRequest = 200,
            HeightRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromRgba(0, 0, 0, 0),
            CornerRadius = 30,
        };
        
        TapGestureRecognizer tap = new TapGestureRecognizer();
        
        _boxView.GestureRecognizers.Add(tap);

        tap.Tapped += (sender, e) =>
        {
            int newRed = _random.Next(256);
            int newGreen = _random.Next(256);
            int newBlue = _random.Next(256);

            _boxView.Color = Color.FromRgb(newRed, newGreen, newBlue);
            _boxView.WidthRequest = _boxView.Width + 20;
            _boxView.HeightRequest = _boxView.Width + 30;

            if (_boxView.WidthRequest > DeviceDisplay.MainDisplayInfo.Width / 3)
            {
                _boxView.WidthRequest = 200;
                _boxView.HeightRequest = 200;
            }
        };
        
        _ellipse = new Ellipse
        {
            WidthRequest = 200,
            HeightRequest = 200,
            Fill = new SolidColorBrush(Color.FromRgb(Blue, Green, Red)),
            Stroke = Colors.BurlyWood,
            StrokeThickness = 5,
            HorizontalOptions = LayoutOptions.Center
        };
        _ellipse.GestureRecognizers.Add(tap);
        
        _polygon = new Polygon
        {
            Points = new PointCollection
            {
                new Point(0,200),
                new Point(100,0),
                new Point(200,200)
            },
            Fill = new SolidColorBrush(Color.FromRgb(Green, Blue, Red)),
            Stroke = Colors.AliceBlue,
            StrokeThickness = 5,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        _horizontalStackLayout = new HorizontalStackLayout { Spacing = 20, HorizontalOptions = LayoutOptions.Center};
        
        for (int j = 0; j < buttons.Count; j++)
        {
            Button button = new Button
            {
                Text = buttons[j],
                FontSize = 28,
                FontFamily = "Luffio",
                TextColor = Colors.BlueViolet,
                BackgroundColor = Colors.LightGray,
                CornerRadius = 10,
                HeightRequest = 50,
                ZIndex = j
            };
            _horizontalStackLayout.Add(button);
            button.Clicked += ChangeFigures;
        }
        
        _verticalStackLayout = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15,
            Children = { _boxView, _ellipse, _polygon },
            HorizontalOptions = LayoutOptions.Center
        };
        
        

        Content = _verticalStackLayout;
    }

    private void ChangeFigures(object? sender, EventArgs e)
    {
        var  button = sender as Button;
        
        if (button.ZIndex == 0)
        {
            Navigation.PushAsync(new TextPage());
        }
        else if (button.ZIndex == 1)
        {
            Navigation.PopToRootAsync();
        }
        else if (button.ZIndex == 2)
        {
            Navigation.PushAsync(new FigurePage());
        }
    }
}