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

    private async void OnDateOrTimeChanged(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate
            .GetValueOrDefault(DateTime.Today);

        int month = selectedDate.Month;

        Color targetCanopyColor = Canopy.BackgroundColor;
        Color targetGroundColor = Ground.Color;
        string statusText = String.Empty;

    switch (month)
        {
            case 12: case 1: case 2:
                targetCanopyColor = Colors.Snow;
                targetGroundColor = Colors.Snow;
                statusText = $"Talv ({selectedDate:dd.MM.yyyy})";
                break;

            case 3: case 4: case 5:
                targetCanopyColor = Color.FromRgb(244, 161, 211);
                targetGroundColor = Color.FromRgb(124, 180, 70);
                statusText = $"Kevad ({selectedDate:dd.MM.yyyy})";
                break;

            case 6: case 7: case 8:
                targetCanopyColor = Color.FromRgb(78, 117, 62);
                targetGroundColor = Colors.DarkOliveGreen;
                statusText = $"Suvi ({selectedDate:dd.MM.yyyy})";
                break;

            case 9: case 10: case 11:
                targetCanopyColor = Color.FromRgb(255, 176, 61);
                targetGroundColor = Color.FromRgb(110, 120, 50);
                statusText = $"Sügis ({selectedDate:dd.MM.yyyy})";
                break;
        }

        StatusLabel.Text = statusText;
        StatusLabel.TextColor = Colors.DarkSlateGray;

        await Task.Delay(300);

        Color currentCanopyColor = Canopy.BackgroundColor;
        Color currentGroundColor = Ground.Color;

        await Task.WhenAll(
            Canopy.ColorToAsync(
                currentCanopyColor,
                targetCanopyColor,
                color => Canopy.BackgroundColor = color,
                1000,
                Easing.CubicInOut),
            Ground.ColorToAsync(
                currentGroundColor,
                targetGroundColor,
                color => Ground.Color = color,
                1000,
                Easing.CubicInOut)
        );
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

    public static Task<bool> ColorToAsync(this View view, Color fromColor, Color toColor, Action<Color> callback,
        uint length = 500, Easing easing = null)
    {
        var tcs = new TaskCompletionSource<bool>();
        easing ??= Easing.Linear;

        var animation = new Animation(v =>
        {
            var r = fromColor.Red + (toColor.Red - fromColor.Red) * v;
            var g = fromColor.Green + (toColor.Green - fromColor.Green) * v;
            var b = fromColor.Blue + (toColor.Blue - fromColor.Blue) * v;
            var a = fromColor.Alpha + (toColor.Alpha - fromColor.Alpha) * v;

            callback(new Color((float)r, (float)g, (float)b, (float)a));
        });

        animation.Commit(view, "ColorAnimation", 16, length, easing, (v, c) => tcs.SetResult(true));
        return tcs.Task;
    }
}