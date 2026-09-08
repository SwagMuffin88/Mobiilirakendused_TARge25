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
    private string message;
    private Grid mainGrid, buttonsGrid;
    
    //List<string> buttons = new List<string>() { "Sisse", "Välja" };
    
    public TrafficLightPage()
    {
        _redEllipse = CreateNewEllipse();
        _yellowEllipse = CreateNewEllipse();
        _greenEllipse = CreateNewEllipse();
        
        // Traffic light ellipses
        var redMessage = "Seisa";
        var yellowMessage = "Ole ootel";
        var greenMessage = "Sõida";

        // Grids for wrapping ellipses and texts
        var redGrid = CreateClickableShape(_redEllipse, "Punane", redMessage);
        var yellowGrid = CreateClickableShape(_yellowEllipse, "Kollane",  yellowMessage);
        var greenGrid = CreateClickableShape(_redEllipse, "Roheline", greenMessage);

        // Vertical stack for the traccif light
        _verticalStackLayout = new VerticalStackLayout
        {
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { redGrid, yellowGrid, greenGrid }
        };
        
        // Botton buttons for managing traffic light state
        var onButton = new Button { Text = "SISSE", BackgroundColor = Colors.LightGray, TextColor = Colors.Black };
        var offButton = new Button { Text = "VÄLJA", BackgroundColor = Colors.LightGray, TextColor = Colors.Black };

        onButton.Clicked += (sender, e) => SwitchTrafficLightState(true);
        offButton.Clicked += (sender, e) => SwitchTrafficLightState(false);

        buttonsGrid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(), new ColumnDefinition() },
            Children = { onButton, offButton }
        };
        
        Grid.SetColumn(offButton, 1);
        
        // Main grid
        mainGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto }
            },
            Children = { _verticalStackLayout, buttonsGrid }
        };
        
        Grid.SetRow(buttonsGrid, 1);
        Content = mainGrid;
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

    private Grid CreateClickableShape(Ellipse ellipse, string text, string messageText)
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
        
        tap.Tapped += async (sender, e) =>
        {
            if (isActive)
            {
                message = messageText;
                await DisplayAlertAsync("Valgusfoor", message, "OK");
            }
            else
            {
                await DisplayAlertAsync("Alert", "Turn on the traffic light", "OK");
            }
        };
        grid.GestureRecognizers.Add(tap);
        return grid;
    }

    private void SwitchTrafficLightState(bool isOn)
    {
        isActive = isOn;

        if (isOn)
        {
            _redEllipse.Fill = new SolidColorBrush(Colors.Red);
            _yellowEllipse.Fill = new SolidColorBrush(Colors.Yellow);
            _greenEllipse.Fill = new SolidColorBrush(Colors.Green);
        }
        else
        {
            _redEllipse.Fill = new SolidColorBrush(Colors.Gray);
            _yellowEllipse.Fill = new SolidColorBrush(Colors.Gray);
            _greenEllipse.Fill = new SolidColorBrush(Colors.Gray);

            message = "Traffic light is switched off";
        }
    }
}