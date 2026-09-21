namespace ex_4;

public partial class PopupPage : ContentPage
{
    public PopupPage()
    {
        Button alertButton = new Button
        {
            Text = "Teade",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };

        alertButton.Clicked += OnAlertButton_Clicked;
        
        Button alertYesNoButton = new Button
        {
            Text = "Jah või ei",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertYesNoButton.Clicked += OnAlertYesNoButton_Clicked;

        Button alertListButton = new Button
        {
            Text = "Valik",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertListButton.Clicked += OnAlertListButton_Clicked;

        Button alertQuestionButton = new Button
        {
            Text = "Küsimus",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };

        alertQuestionButton.Clicked += OnAlertQuestionButton_Clicked;
        
        Content = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = new Thickness(0, 50, 0, 0),
            Children = { alertButton, alertYesNoButton, alertListButton, alertQuestionButton }
        };
    }
    private async void OnAlertButton_Clicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Teade", "Teil on uus teade", "OK");
    }

    private async void OnAlertYesNoButton_Clicked(object? sender, EventArgs e)
    {
        bool result = await DisplayAlertAsync("Kinnitus", "Kas oled kindel", "Jah", "Ei");

        await DisplayAlertAsync("Teade", "Te valisite: " + (result ? "Jah" : "Ei"), "OK");
    }
    
    private async void OnAlertListButton_Clicked(object? sender, EventArgs e)
    {
        string action = await DisplayActionSheetAsync("Mida te soovite teha?", "Loobu", "Kustutada", 
            "Tantsida", "Laulda", "Joonistada"
            );

        if (action != null && action != "Loobu")
        {
            await DisplayAlertAsync("Valik", "Teie valik: " + action, "OK");
        }
    }

    private async void OnAlertQuestionButton_Clicked(object? sender, EventArgs e)
    {
        string result1 = await DisplayPromptAsync("Küsimus", "Kuidas läheb?", "Hästi!");
        string result2 = await DisplayPromptAsync("Vasta", "Millega võrdub 5 + 5?", initialValue:
            "10", maxLength: 2, keyboard: Keyboard.Numeric);
    }
}