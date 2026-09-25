namespace praktika_3;

public partial class GamePage : ContentPage
{
    private HorizontalStackLayout _actionButtonsLayout;
    private VerticalStackLayout _mainLayout;
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

        btnNewGame.Clicked += ClearGameBoard;
        
        var btnWhoStarts = new Button
        {
            Text = "Kes alustab?",
            Margin = new Thickness(5)
        };
        
        btnWhoStarts.Clicked += OnWhoStartsClicked;

        _actionButtonsLayout = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center,
            Children = { btnNewGame, btnWhoStarts }
        };
        
        _mainLayout = new VerticalStackLayout{ 
            Spacing = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { _gameBoard, _actionButtonsLayout }
        };
        
        Content = _mainLayout;
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

        if (CheckWin())
        {
            _isGameActive = false;
            bool playAgain = await DisplayAlertAsync(
                "Mäng läbi!", 
                $"{_currentPlayer} võitis! Kas soovid veel mängida?", 
                "Jah", 
                "Ei"
            );
            
            if (playAgain) ClearGameBoard();
        }
        
        _currentPlayer = (_currentPlayer == "X") ? "0" : "X";
    }

    private async void OnWhoStartsClicked(object? sender, EventArgs e)
    {
        ClearGameBoard();
        _currentPlayer = _random.Next(0, 2) == 0 ? "X" : "O";
        
        await DisplayAlertAsync(
            "Esimese käigu tegija", 
            $"Mängu alustab: {_currentPlayer}", 
            "OK"
            );
    }
    
    private void ClearGameBoard(object? sender = null, EventArgs? e = null)
    {
        _isGameActive = true;
        _currentPlayer = "X";

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                _boardButtons[row, col].Text = "";
            }
        }
    }

    private bool CheckWin()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!string.IsNullOrEmpty(_boardButtons[i, 0].Text) &&
                _boardButtons[i, 0].Text == _boardButtons[i, 1].Text &&
                _boardButtons[i, 1].Text == _boardButtons[i, 2].Text )
            {
                return true;
            }
            
            if (!string.IsNullOrEmpty(_boardButtons[0, i].Text) &&
               _boardButtons[0, i].Text == _boardButtons[1, i].Text &&
               _boardButtons[1, i].Text == _boardButtons[2, i].Text)
            {
                return true;
            }
        }

        if (!string.IsNullOrEmpty(_boardButtons[0, 0].Text) &&
            _boardButtons[0, 0].Text == _boardButtons[1, 1].Text &&
            _boardButtons[1, 1].Text == _boardButtons[2, 2].Text)
        {
            return true;
        }

        if (!string.IsNullOrEmpty(_boardButtons[2, 0].Text) &&
            _boardButtons[2, 0].Text == _boardButtons[1, 1].Text &&
            _boardButtons[1, 1].Text ==  _boardButtons[2, 2].Text)
        {
            return true;
        }

        return false;
    }
}