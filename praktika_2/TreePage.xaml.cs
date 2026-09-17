using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace praktika_2;

public partial class TreePage : ContentPage
{
    private uint _animationDuration = 1000;
    public TreePage()
    {
        InitializeComponent();
    }

    private async void OnActionClicked(object sender, EventArgs e)
    {
        if (ActionPicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "Vali kõigepealt tegevus!";
            StatusLabel.TextColor = Colors.DarkRed;
            return;
        }

        string selectedAction = ActionPicker.SelectedItem.ToString();
        StatusLabel.TextColor = Colors.DarkGreen;
        
        switch(selectedAction)
        {
            case "Kasvata":
                StatusLabel.Text = "Puu kasvab!";
                double canopyIncrement = 20;
                double trunkHeightIncrement = 20;
                double trunkWidthIncrement = 4;

                await Task.WhenAll(
                    Canopy.AnimateSizeChangeAsync(
                        Canopy.WidthRequest + canopyIncrement,
                        Canopy.HeightRequest + canopyIncrement,
                        _animationDuration),

                    Trunk.AnimateSizeChangeAsync(
                        Trunk.WidthRequest + trunkWidthIncrement,
                        Trunk.HeightRequest + trunkHeightIncrement,
                        _animationDuration)
                );
                break;
            
            case  "Lase õitsema":
                StatusLabel.Text = "Puu õitseb!";
                Canopy.BackgroundColor = Color.FromRgb(244, 161, 211);
                break;
            
            case "Raputa":
                StatusLabel.Text = "Puu väriseb!";
                
                // Liigutab puuvõra horisontaalselt edasi-tagasi
                await Canopy.TranslateToAsync(-10, 0, 50);
                await Canopy.TranslateToAsync(10, 0, 50);
                await Canopy.TranslateToAsync(-5, 0, 50);
                await Canopy.TranslateToAsync(0, 0, 50);
                break;
            
            case "Langeta":
                StatusLabel.Text = "";
                // TODO add falling and dissappearing animation
                break;
        }
    }

    private void OnOpacitySliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        Canopy.Opacity = e.NewValue;
        OpacityValueLabel.Text = $"Võra läbipaistvus: {e.NewValue:F2}";
    }
    
    private void OnSpeedStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        _animationDuration = (uint)e.NewValue;
        SpeedLabel.Text = $"Animatsiooni kestus: {_animationDuration} ms";
    }
    
    private void OnDateOrTimeChanged(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate
            .GetValueOrDefault(DateTime.Today);
        
        int month = selectedDate.Month;

        switch (month)
        {
            case 12: case 1: case 2:
                Canopy.BackgroundColor = Colors.Snow;
                StatusLabel.Text = $"Talv ({selectedDate:dd.MM.yyyy})";
                break;

            case 3: case 4: case 5:
                Canopy.BackgroundColor = Color.FromRgb(244, 161, 211);
                StatusLabel.Text = $"Kevad ({selectedDate:dd.MM.yyyy})";
                break;

            case 6: case 7: case 8:
                Canopy.BackgroundColor = Colors.ForestGreen;
                StatusLabel.Text = $"Suvi ({selectedDate:dd.MM.yyyy})";
                break;

            case 9: case 10: case 11:
                Canopy.BackgroundColor = Colors.DarkOrange;
                StatusLabel.Text = $"Sügis ({selectedDate:dd.MM.yyyy})";
                break;
        }

        StatusLabel.TextColor = Colors.DarkSlateGray;
    }
    
    private void OnTimePickerPropertyChanged(object sender, EventArgs e)
    {
        
    }
    
}
public static class ViewExtensions
{
    public static Task<bool> AnimateSizeChangeAsync(this View view, double newWidth, double newHeight, uint length)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();
        var startWidth = view.WidthRequest;
        var startHeight = view.HeightRequest;

        var animation = new Animation(v =>
        {
            view.WidthRequest = startWidth + (newWidth - startWidth) * v;
            view.HeightRequest = startHeight + (newHeight - startHeight) * v;
        });

        animation.Commit(view, "SizeAnimation", 16, length, null, (v, c) => taskCompletionSource.SetResult(true));
        return taskCompletionSource.Task;
    }
}