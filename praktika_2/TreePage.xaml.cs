using System.ComponentModel;

namespace praktika_2;

public partial class TreePage : ContentPage
{
    private uint _animationDuration = 1000;
    private bool _isInitialized = false;
    
    public TreePage()
    {
        InitializeComponent();
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();

        DateTime now = DateTime.Now;
        VirtualDatePicker.Date = now.Date;
        VirtualTimePicker.Time = now.TimeOfDay;
        _isInitialized = true;
        
        OnDateOrTimeChanged(VirtualDatePicker, new DateChangedEventArgs(now.Date, now.Date));

        ApplyTimeOfDayLighting(now.TimeOfDay);
    }

    private async void OnActionClicked(object sender, EventArgs e)
    {
        try
        {
            if (ActionPicker.SelectedIndex == -1)
            {
                StatusLabel.Text = "Vali kõigepealt tegevus!";
                StatusLabel.TextColor = Colors.DarkRed;
                return;
            }

            string selectedAction = ActionPicker.SelectedItem.ToString();


            int currentMonth = VirtualDatePicker.Date?.Month ?? DateTime.Now.Month;
            TimeSpan currentTime = VirtualTimePicker.Time.GetValueOrDefault(TimeSpan.FromHours(12));

            bool isWinter = currentMonth == 12 || currentMonth <= 2;
            bool isAutumnOrWinter = isWinter || (currentMonth >= 9 && currentMonth <= 11);
            bool isNightTime = currentTime.Hours >= 22 || currentTime.Hours < 6;

            if (selectedAction == "Lase õitsema" && isAutumnOrWinter)
            {
                StatusLabel.Text = "Puu ei saa õitseda sügisel ega talvel!";
                StatusLabel.TextColor = Colors.DarkRed;

                await ShakeStatusLabelAsync();
                return;
            }

            if (selectedAction == "Langeta" && (isNightTime || !isWinter))
            {
                StatusLabel.Text = "Puud saab langetada vaid talvel ja valgel ajal!";
                StatusLabel.TextColor = Colors.DarkRed;

                await ShakeStatusLabelAsync();
                return;
            }

            StatusLabel.TextColor = Colors.DarkGreen;

            switch (selectedAction)
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

                case "Lase õitsema":
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
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"An unexpected error has occurred: {exception.Message}");
            StatusLabel.Text = "Viga rakenduse töös.";
            StatusLabel.TextColor = Colors.Red;
        }
    }

    private async Task ShakeStatusLabelAsync()
    {
        await StatusLabel.TranslateTo(-5, 0, 50);
        await StatusLabel.TranslateTo(5, 0, 50);
        await StatusLabel.TranslateTo(0, 0, 50);
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
        try
        {
            DateTime selectedDate = e.NewDate
                .GetValueOrDefault(DateTime.Today);

            int month = selectedDate.Month;

            Color targetCanopyColor = Canopy.BackgroundColor;
            Color targetGroundColor = Ground.Color;
            string statusText = String.Empty;

            switch (month)
            {
                case 12:
                case 1:
                case 2:
                    targetCanopyColor = Colors.Snow;
                    targetGroundColor = Colors.Snow;
                    statusText = $"Talv ({selectedDate:dd.MM.yyyy})";
                    break;

                case 3:
                case 4:
                case 5:
                    targetCanopyColor = Color.FromRgb(244, 161, 211);
                    targetGroundColor = Color.FromRgb(124, 180, 70);
                    statusText = $"Kevad ({selectedDate:dd.MM.yyyy})";
                    break;

                case 6:
                case 7:
                case 8:
                    targetCanopyColor = Color.FromRgb(78, 117, 62);
                    targetGroundColor = Colors.DarkOliveGreen;
                    statusText = $"Suvi ({selectedDate:dd.MM.yyyy})";
                    break;

                case 9:
                case 10:
                case 11:
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
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"An unexpected error has occurred: {exception.Message}");
            StatusLabel.Text = "Viga rakenduse töös.";
            StatusLabel.TextColor = Colors.Red;
        }
    }
    
    private async void OnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        try
        {
            if (!_isInitialized)
                return;

            if (e.PropertyName == nameof(TimePicker.Time))
            {
                TimeSpan selectedTime = VirtualTimePicker.Time ?? TimeSpan.FromHours(12);
                await ApplyTimeOfDayLighting(selectedTime);
            }
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"An unexpected error has occurred: {exception.Message}");
            StatusLabel.Text = "Viga rakenduse töös.";
            StatusLabel.TextColor = Colors.Red;
        }
    }

    private async Task ApplyTimeOfDayLighting(TimeSpan time)
    {
        int hour = time.Hours;
        Color targetOverlayColor;
        double targetOpacity;
        string dayTimeName = "";

        if (hour >= 22 || hour < 6)
        {
            targetOverlayColor = Color.FromRgb(10, 15, 40);
            targetOpacity = 0.7;
            dayTimeName = "Öine aeg";
        }
        else if ((hour >= 6 && hour < 8) || (hour >= 19 && hour < 22))
        {
            targetOverlayColor = Color.FromRgb(230, 108, 62);
            targetOpacity = 0.3;

            dayTimeName = (hour < 8) ? "Varahommik" : "Õhtu";
        }
        else
        {
            targetOverlayColor = Colors.Black;
            targetOpacity = 0.0;
            dayTimeName = "Päevane aeg";
        }
            
        StatusLabel.Text = $"{dayTimeName} ({time:hh\\:mm})";

        if (targetOpacity > 0.0)
        {
            DarknessOverlay.Color = targetOverlayColor;
            DarknessOverlay.IsVisible = true;
            await DarknessOverlay.FadeToAsync(targetOpacity, 400, Easing.CubicInOut);
        }
        else 
        {
            await DarknessOverlay.FadeToAsync(0.0, 400, Easing.CubicInOut);
            DarknessOverlay.IsVisible = false;
        }
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