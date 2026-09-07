using ex_2;

public partial class StartPage : ContentPage
{
    VerticalStackLayout vst;
    ScrollView sv;

    public List<ContentPage> Pages = new List<ContentPage>()
    {
        //new TextPage(),
        //new FigurePage()
    };

    public List<string> PageNames = new List<string>()
    {
        "Tekst",
        "Kujund"
    };

    public StartPage()
    {
        //Title = "Avaleht";
        vst = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15
        };

        for (int i = 0; i < Pages.Count; i++)
        {
            Button button = new Button
            {
                Text = PageNames[i],
                FontSize = 36,
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 10,
                HeightRequest = 60,
                ZIndex = i
            };

            vst.Add(button);

            button.Clicked += (sender, e) =>
            {
                var choice = Pages[button.ZIndex];
                Navigation.PushAsync(choice);
            };
        }

        sv = new ScrollView
        {
            Content = vst
        };
        
        Content = sv;
    }
}