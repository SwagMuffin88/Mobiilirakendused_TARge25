using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_2;

public partial class TimerPage : ContentPage
{
    public TimerPage()
    {
        InitializeComponent();
    }
    
    private bool _isOff = false;

    private async void ShowTime()
    {
        while (_isOff)
        {
            timer_button.Text = DateTime.Now.ToLongTimeString();
            await Task.Delay(1000);
        }
    }
    private void HandleGestureRecognizerTapped(object? sender, TappedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void HandleTimerButtonOnClicked(object? sender, EventArgs e)
    {
        if (_isOff)
        {
            _isOff = false;
        }
        else
        {
            _isOff = true;
            ShowTime();
        }
    }
    
    private void HandleBackButtonClick(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}