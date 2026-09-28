using System.Data;

namespace praktika_3;

public partial class ScorePage : ContentPage
{
    private Label _labelStats;
    
    public ScorePage()
    {
        Title = "Mängu punktiseis";

        _labelStats = new Label
        {
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Center
        };

        var btnReset = new Button
        {
            Text = "Lähtesta punktiseis",
            HorizontalOptions = LayoutOptions.Center
        };
        
        btnReset.Clicked += (s, e) =>
        {
            ScoreService.ResetScore();
            UpdateStats();
        };

        Content = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = 30,
            VerticalOptions = LayoutOptions.Center,
            Children = { _labelStats, btnReset }
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateStats();
    }

    private void UpdateStats()
    {
        _labelStats.Text = $"X võidud: {ScoreService.X_Wins}\n" +
                           $"O võidud: {ScoreService.O_Wins}\n" +
                           $"Viigid: {ScoreService.Draws}";
    }
}