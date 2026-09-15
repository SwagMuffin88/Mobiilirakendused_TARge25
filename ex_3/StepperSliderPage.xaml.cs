using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;

namespace ex_3;

public partial class StepperSliderPage : ContentPage
{
    private Label _label;
    private Stepper _stepper;
    private Slider _slider;
    private AbsoluteLayout _absoluteLayout;
    
    public StepperSliderPage()
    {
        _label = new Label
        {
            Text = "...",
            BackgroundColor = Colors.LightBlue
        };

        _stepper = new Stepper
        {
            Minimum = 0,
            Maximum = 360,
            Increment = 5,
            Value = 50,
            HorizontalOptions = LayoutOptions.Center
        };

        _stepper.ValueChanged += handleStepperSliderChange;
        
        _slider = new Slider
        {
            Minimum = 0,
            Maximum = 360,
            Value = 50,
            HorizontalOptions = LayoutOptions.Center,
            MinimumTrackColor = Colors.LightGray,
            MaximumTrackColor = Colors.DarkGray
        };

        _slider.ValueChanged += handleStepperSliderChange;

        _absoluteLayout = new AbsoluteLayout
            {
                Children = { _label, _stepper, _slider }
            };

        List<View> controls = new List<View> { _label, _stepper, _slider };

        for (int i = 0; i < controls.Count; i++)
        {
            double y = 0.2 + i * 0.2;
            AbsoluteLayout.SetLayoutBounds(controls[i], new Rect(0.5, y, 300, 60));
            AbsoluteLayout.SetLayoutFlags(controls[i], AbsoluteLayoutFlags.PositionProportional);
        }

        Content = _absoluteLayout;
    }

    private void handleStepperSliderChange(object? sender, ValueChangedEventArgs e)
    {
        var newValue = e.NewValue;
        var newRgbElementValue = newValue * 2.55;
        
        _label.Text = $"Stepperi / slideri väärtus: {newValue:F0}";
        _label.FontSize = 24 + newValue / 4;
        _label.BackgroundColor = Color.FromRgb(newRgbElementValue, 255 - newRgbElementValue, 128);
        _label.TextColor = Color.FromRgb(255 - newRgbElementValue, newRgbElementValue, 128);
        _label.Rotation = newValue;
    }
}