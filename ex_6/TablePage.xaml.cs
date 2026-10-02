namespace ex_6;

public partial class TablePage : ContentPage
{
    private TableView _tableView;
    private TableSection photoSection;
    private Switch _switch;
    private ViewCell switchCell;
    private ViewCell imageCell;
    private Image photoImage;
    private Label photoTitleLabel;
    private Label photoDetailLabel;
    
    public TablePage()
    {
        var contactSection = new TableSection("Kontaktandmed:")
        {
            // Replacement for EntryCell:
            // Phone number input
            new ViewCell
            {
                View = new HorizontalStackLayout
                {
                    Padding = new Thickness(15, 5),
                    Spacing = 10,
                    Children =
                    {
                        new Label { Text = "Telefon", VerticalOptions = LayoutOptions.Center, WidthRequest = 80 },
                        new Entry
                        {
                            Placeholder = "Sisesta telefoninumber",
                            Keyboard = Keyboard.Telephone,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }
                    }
                }
            },

            // Email input
            new ViewCell
            {
                View = new HorizontalStackLayout
                {
                    Padding = new Thickness(15, 5),
                    Spacing = 10,
                    Children =
                    {
                        new Label { Text = "Email", VerticalOptions = LayoutOptions.Center, WidthRequest = 80 },
                        new Entry
                        {
                            Placeholder = "Sisesta email",
                            Keyboard = Keyboard.Email,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        }
                    }
                }
            }
        };
        
        // Replacement for SwitchCell:
        _switch = new Switch();

        _switch.Toggled += SwitchOnChanged;

        switchCell = new ViewCell
        {
            View = new HorizontalStackLayout
            {
                Padding = new Thickness(15, 5),
                Spacing = 10,
                Children =
                {
                    new Label { Text = "Näita veel", VerticalOptions = LayoutOptions.Center },
                    _switch
                }
            }
        };

        imageCell = new ViewCell
        {
            View = new HorizontalStackLayout
            {
                Padding = new Thickness(10),
                Spacing = 10,
                Children =
                {
                    new Image { Source = ImageSource.FromFile("bob.jpg"), WidthRequest = 50, HeightRequest = 50 },
                    new VerticalStackLayout
                    {
                        Children =
                        {
                            new Label { Text = "Foto nimetus", FontAttributes = FontAttributes.Bold },
                            new Label { Text = "Foto kirjeldus", FontSize = 12 }
                        }
                    }
                }
            }
        };

        photoSection = new TableSection();
        
        photoSection.Add(switchCell);
        photoSection.Add(imageCell);

        _tableView = new TableView
        {
            Intent = TableIntent.Form,
            Root = new TableRoot
            {
                contactSection,
                photoSection
            }
        };

        Content = _tableView;
    }
    private void SwitchOnChanged(object sender, ToggledEventArgs e)
    {
        Label showMoreLabel = new Label();
        if (e.Value)
        {
            photoSection.Title = "Foto";
            photoSection.Add(imageCell);
            showMoreLabel.Text = "Peida";
        }
        else
        {
            photoSection.Title = "";
            photoSection.Remove(imageCell);
            showMoreLabel.Text = "Näita veel";
        }
    }
}