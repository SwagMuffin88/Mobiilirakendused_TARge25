namespace praktika_3;

public partial class GamePage : ContentPage
{
    private HorizontalStackLayout _horizontalStackLayout;
    private VerticalStackLayout _verticalStackLayout;
    private Grid _gameBoard;
    private bool _isGameActive = true;
    private string _currentPlayer = "X";
    private readonly Random _random = new Random();
    private Button[,] _boardButtons = new Button[3, 3];
    
    
    public GamePage()
    {
        InitializeComponent();
        
        _gameBoard = CreateGameBoard();

        var btnNewGame = new Button
        {
            Text = "Uus mäng",
            Margin = new Thickness(5)
        };

        
        _verticalStackLayout = new VerticalStackLayout{ 
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { _gameBoard }
        };
        
        Content = _verticalStackLayout;
    }

    private Grid CreateGameBoard()
    {
        Grid grid = new Grid
        {
            WidthRequest = 300,
            HeightRequest = 300,
            RowSpacing = 5,
            ColumnSpacing = 5,
            BackgroundColor = Colors.LightGray
        };
        
        for (int i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                Button button = new Button
                {
                    Text = "",
                    FontSize = 32,
                    FontAttributes = FontAttributes.Bold,
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black
                };

                button.Clicked += OnCellClicked;
                
                _boardButtons[row, col] = button;
                grid.Add(button, col, row);
            }
        }

        return grid;
    }

    private async void OnCellClicked(object? sender, EventArgs e)
    {
        if (!_isGameActive || sender is not Button clickedButton)
            return;
        
        if (!string.IsNullOrEmpty(clickedButton.Text))
            return;

        clickedButton.Text = _currentPlayer;
        
        // TODO win/tie check
        
        _currentPlayer = (_currentPlayer == "X") ? "0" : "X";
    }
}