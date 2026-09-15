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
                await Trunk.ScaleToAsync(1.2, _animationDuration / 2);
                await Canopy.ScaleToAsync(1.0, _animationDuration / 2);
                break;
            
            case  "Lase õitseda":
                StatusLabel.Text = "Puu õitseb!";
                Canopy.BackgroundColor = Color.FromRgb(244, 161, 211);
                break;
            
            case "Värista":
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

    private void OnOpacitySliderValueChanged(object sender, EventArgs e)
    {
        
    }
    
    private void OnSpeedStepperValueChanged(object sender, EventArgs e)
    {
        
    }
    private void OnDateOrTimeChanged(object sender, EventArgs e)
    {
        
    }
    
    private void OnTimePickerPropertyChanged(object sender, EventArgs e)
    {
        
    }
    
}