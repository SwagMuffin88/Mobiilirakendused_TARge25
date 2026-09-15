using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;

namespace ex_3;

public partial class PickerPage : ContentPage
{
    DatePicker _datePicker;
    TimePicker _timePicker;
    Label _dateTimeLabel;
    AbsoluteLayout _absoluteLayout;
    
    public PickerPage()
    {
        _datePicker = new DatePicker
        {
            MinimumDate = DateTime.Now.AddDays(-15),
            MaximumDate = DateTime.Now.AddDays(15),
            Date = DateTime.Now,
            HorizontalOptions = LayoutOptions.Center,
            Format = "D"
        };

        _datePicker.DateSelected += (sender, e) =>
        {
            _dateTimeLabel.Text = $"Valitud kuupäev: \n{_datePicker.Date:D}";
        };

        _timePicker = new TimePicker
        {
            Time = DateTime.Now.TimeOfDay,
            HorizontalOptions = LayoutOptions.Center,
            Format = "T"
        };

        _timePicker.PropertyChanged += (sender, e) =>
        {
            _dateTimeLabel.Text = $"Valitud kellaaeg: \n{_timePicker.Time:T}";
        };

        _dateTimeLabel = new Label
        {
            Text = "Vali kuupäev või kellaaeg",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        _absoluteLayout = new AbsoluteLayout
        {
            Children =
            {
                _datePicker, _timePicker, _dateTimeLabel
            }
        };
        
        List<View> controls = new List<View> { _datePicker, _timePicker, _dateTimeLabel };
        
        for (int i = 0; i < controls.Count; i++)
        {
            double y = 0.2 + i * 0.2;
            AbsoluteLayout.SetLayoutBounds(controls[i], new Rect(0.5, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(controls[i], AbsoluteLayoutFlags.PositionProportional);
        }

        Content = _absoluteLayout;
    }
}