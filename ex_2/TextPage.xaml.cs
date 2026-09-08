namespace ex_2;

public partial class TextPage : ContentPage
{
    Label _label;
    Editor _editor;
    HorizontalStackLayout _horizontalStack;
    VerticalStackLayout _verticalStack;
    
    List<string> buttons = new List<string>()
    {
        "Back",
        "Home",
        "Next"
    };
    
    public TextPage()
    {
        _label = new Label
        {
            Text = "Pealkiri",
            FontSize = 36,
            FontFamily = "Luffio",
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold
        };
        
        _editor = new Editor
        {
            Placeholder = "Sisesta tekst...",
            PlaceholderColor = Colors.Red,
            FontSize = 18,
            FontAttributes = FontAttributes.Italic,
            HorizontalOptions = LayoutOptions.Center,
        };

        _editor.TextChanged += (sender, e) =>
        {
            _label.Text = _editor.Text;
        };

        _horizontalStack = new HorizontalStackLayout()
        {
            Spacing = 20, 
            HorizontalOptions = LayoutOptions.Center
        };
        
        for (int j = 0; j < buttons.Count; j++)
        {
            Button button = new Button
            {
                Text = buttons[j],
                FontSize = 28,
                FontFamily = "Luffio",
                TextColor = Colors.BlueViolet,
                BackgroundColor = Colors.LightGray,
                CornerRadius = 10,
                HeightRequest = 50,
                ZIndex = j
            };
            _horizontalStack.Add(button);
            button.Clicked += ChangePage;
        }

        _verticalStack = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15,
            Children = { _label, _editor, _horizontalStack },
            HorizontalOptions = LayoutOptions.Center
        };

        Content = _verticalStack;

    }

    private void ChangePage(object? sender, EventArgs e)
    {
        var  button = sender as Button;
        
        if (button.ZIndex == 0)
        {
            Navigation.PopAsync();
        }
        else if (button.ZIndex == 1)
        {
            Navigation.PopToRootAsync();
        }
        else if (button.ZIndex == 2)
        {
            Navigation.PushAsync(new FigurePage());
        }
    }
}