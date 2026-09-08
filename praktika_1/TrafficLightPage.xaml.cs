using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;

namespace praktika_1;

public partial class TrafficLightPage : ContentPage
{
    private Ellipse _redEllipse, _yellowEllipse, _greenEllipse;
    private bool isActive = false;
    private HorizontalStackLayout _horizontalStackLayout;
    private VerticalStackLayout _verticalStackLayout;
    
    //List<string> buttons = new List<string>() { "Sisse", "Välja" };
    
    public TrafficLightPage()
    {
        _redEllipse = CreateNewEllipse();
        _yellowEllipse = CreateNewEllipse();
        _greenEllipse = CreateNewEllipse();

        var redGrid = CreateClickableShape(_redEllipse, "red", Colors.Red);
    }

    private Ellipse CreateNewEllipse()
    {
        return new Ellipse
        {
            WidthRequest = 180,
            HeightRequest = 180,
            Fill = new SolidColorBrush(Colors.Gray),
            HorizontalOptions = LayoutOptions.Center
        };
    }

    private Grid CreateClickableShape(Ellipse ellipse, string text, Color activeColor)
    {
        var grid = new Grid
        {
            Children =
            {
                ellipse,
                new Label
                {
                    Text = text,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Black
                }
            }
        };
        var tap = new TapGestureRecognizer();
        tap.Tapped += (sender, e) =>
        {
            if (isActive)
            {
                ellipse.Fill = new SolidColorBrush(activeColor);
            }
            else
            {
                DisplayAlertAsync("Alert", "Turn on the traffic light", "OK");
            }
        };
        grid.GestureRecognizers.Add(tap);
        return grid;
    }
}