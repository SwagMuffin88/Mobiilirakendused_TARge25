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
        int red = _random.Next(256);
        int green = _random.Next(256);
        int blue = _random.Next(256);
        
        _boxView = new BoxView
        {
            Color = Color.FromRgb(red, green, blue),
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
            Fill = new SolidColorBrush(Color.FromRgb(blue, green, red)),
            Stroke = Colors.BurlyWood,
            StrokeThickness = 5,
            HorizontalOptions = LayoutOptions.Center
        };
        _ellipse.GestureRecognizers.Add(tap);
        
        var a = new Point(0, 200);
        var b = new Point(100, 0);
        var c = new Point(200, 200);

        _polygon = new Polygon
        {
            Points = new PointCollection { a, b, c },
            Fill = new SolidColorBrush(Color.FromRgb(green, blue, red)),
            Stroke = Colors.AliceBlue,
            StrokeThickness = 5,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        
        TapGestureRecognizer _polygonTap = new TapGestureRecognizer();
        _polygonTap.NumberOfTapsRequired = 2; // Double tap
        _polygon.GestureRecognizers.Add(_polygonTap);

        _polygonTap.Tapped += (sender, e) =>
        {
            red = _random.Next(256); 
            green = _random.Next(256);
            blue = _random.Next(256);

            _polygon.Fill = new SolidColorBrush(Color.FromRgb(red, green, blue));
            _polygon.Points = new PointCollection
            {
                new Point(0, red),
                new Point(green, 0),
                new Point(green, blue),
            };
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

    private bool _isOff = false;

    private async void showTime()
    {
        while (_isOff)
        {
            await Task.Delay(1000);
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
        }
    }
}