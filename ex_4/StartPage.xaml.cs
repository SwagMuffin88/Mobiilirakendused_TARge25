namespace ex_4;

[System.Runtime.Versioning.SupportedOSPlatform("android21.0")]
public partial class StartPage : ContentPage
{
    VerticalStackLayout verticalStackLayout;
    ScrollView scrollView;
    
    public List<ContentPage> Pages = new List<ContentPage>()
    {
        new TestPage()
    };
    public List<string> PageNames = new List<string>() { "Test" };
    public StartPage()
    {
        verticalStackLayout = new VerticalStackLayout { Padding=20, Spacing=15 };
        for (int i=0; i < Pages.Count; i++)
        {
            Button nupp = new Button
            {
                Text = PageNames[i],
                FontSize = 36,
                FontFamily="Luffio",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10,
                HeightRequest = 60,
                ZIndex = i
            };
            verticalStackLayout.Add(nupp);
            nupp.Clicked += (sender, e) =>
            {
                var valik = Pages[nupp.ZIndex];
                Navigation.PushAsync(valik);
            };
        }

        Button resetButton = new Button
        {
            Text = "Nulli seaded",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            CornerRadius = 10,
            HeightRequest = 50,
            Margin = new Thickness(0, 30, 0, 0)
        };

        resetButton.Clicked += async (sender, e) =>
        {
            Preferences.Default.Remove("FirstInit");
            await DisplayAlertAsync("Edukalt nullitud!", "Mälu on tühjendatud.", "OK");
        };

        scrollView = new ScrollView { Content = verticalStackLayout };
        Content = scrollView;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool isFirstOnInit = Preferences.Default.Get("FirstInit", true);

        if (isFirstOnInit)
        {
            bool result = await DisplayAlertAsync(
                "Tere tulemast! ",
                "Kas soovite näha rakenduse tutvustust?",
                "Jah",
                "Ei"
            );
            if (result)
            {
                await DisplayAlertAsync(
                    "Juhend",
                    "Vali nimekirjast sobiv teema ja uuri, kuidas elemendid töötavad.",
                    "OK"
                    );
            }
            Preferences.Default.Set("FirstInit", false);
        }
    }
}