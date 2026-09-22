using ex_2;
using ex_3;
using praktika_1;

namespace ex_4;

[System.Runtime.Versioning.SupportedOSPlatform("android21.0")]
public partial class StartPage : ContentPage
{
    VerticalStackLayout verticalStackLayout;
    ScrollView scrollView;
    
    public List<ContentPage> Pages = new List<ContentPage>()
    {
        new TextPage(), new FigurePage(), new TrafficLightPage(), new PickerPage(), new StepperSliderPage()
    };
    public List<string> PageNames = new List<string>() { "Tekst", "Kujund", "Valgusfoor", "Kuupäev/Aeg", "Liugur" };
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

        scrollView = new ScrollView { Content = verticalStackLayout };
        Content = scrollView;
    }
}