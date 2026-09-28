namespace praktika_3;

public partial class GamePage : ContentPage
{
    private HorizontalStackLayout _actionButtonsLayout;
    private VerticalStackLayout _mainLayout;
    private Grid _gameBoard;
    private bool _isGameActive = true;
    
    private string _currentPlayer = "X";
    private string _playerSymbol = "X";
    private string _computerSymbol = "O";
    
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
        if (!_isGameActive || sender is not Button clickedButton || _currentPlayer != _playerSymbol) 
            return;

        if (!string.IsNullOrEmpty(clickedButton.Text))
            return;

        clickedButton.Text = _playerSymbol;

        if (CheckWin() || IsBoardFull())
        {
            await EndCurrentGame();
        }
        else
        {
            _currentPlayer = _computerSymbol;
            await MakeComputerMove(_computerSymbol);
        } 
    }

    private async void OnWhoStartsClicked(object? sender, EventArgs e)
    {
        ClearGameBoard();
        _currentPlayer = _random.Next(0, 2) == 0 ? "X" : "O";

        _playerSymbol = _currentPlayer;
        _computerSymbol = (_playerSymbol == "X") ? "O" : "X";
        
        await DisplayAlertAsync(
            "Esimese käigu tegija", 
            $"Mängu alustab: {_currentPlayer}", 
            "OK"
            );

        if (_currentPlayer == _computerSymbol)
        {
            await MakeComputerMove(_computerSymbol);
        }
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

    private bool IsBoardFull()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(_boardButtons[row, col].Text)) return false;
            }
        }

        return true;
    }

    private async Task MakeComputerMove(string symbol)
    {
        if (!_isGameActive) return;
        
        var freeCells = new List<(int row, int col)>();

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (string.IsNullOrEmpty(_boardButtons[r, c].Text))
                {
                    freeCells.Add((r, c));
                }
            }
        }

        if (freeCells.Count == 0) return;

        await Task.Delay(400);

        var (row, col) = freeCells[_random.Next(freeCells.Count)];

        _boardButtons[row, col].Text = symbol;

        if (CheckWin() || IsBoardFull()) 
            await EndCurrentGame();
        else 
            _currentPlayer = _playerSymbol;
    }

    private async Task EndCurrentGame()
    {
        try
        {
            _isGameActive = false;
            string displayMessage = "";
            
            if (CheckWin())
            {
                AddScore();
                displayMessage = $"{_currentPlayer} võitis! Kas soovid veel mängida?";
            }
            
            else if (IsBoardFull())
            {
                ScoreService.Draws++;
                displayMessage = "Mäng lõppes viigiga! Kas soovid veel mängida?";
            }

            bool playAgain = await DisplayAlertAsync("Mäng läbi!", displayMessage,
                "Jah", "Ei"
            );

            if (playAgain) ClearGameBoard();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
        }
    }

    private void AddScore()
    {
        switch (_currentPlayer)
        {
            case "X":
                ScoreService.X_Wins++;
                break;
            case "O":
                ScoreService.O_Wins++;
                break;
        }
    }
}