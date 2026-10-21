using praktika_5.Game;
using praktika_5.Models;

namespace praktika_5;

public partial class MainPage : ContentPage
{
    private GameManager _gameManager;
    private Theme _currentTheme;

    private const int Rows = 10;
    private const int Cols = 10;
    
    private Button[,] _cellButtons;

    public MainPage()
    {
        InitializeComponent();
        
        // Default theme: light
        _currentTheme = new Theme("Hele", Colors.White, Colors.Black, 
            Colors.RoyalBlue, Colors.LightGray, "OpenSansRegular");
        
        _currentTheme.Apply(this);
        
        var player = new Player("Mängija 1");
        PlayerNameLabel.Text = player.Name;
        
        _gameManager = new GameManager(player, _currentTheme, Rows, Cols);
        
        _gameManager.BoardUpdated += OnBoardUpdated;
        
        _gameManager.StartNewGame();
        
        InitializeBoard();
    }

    private async void OnNewGameClicked(object sender, EventArgs e)
    {
        SoundHelper.PlayClickSound();
        await DisplayAlertAsync("Uus mäng", "Mängulaud uuendatud!", "OK");
        
        if (sender is Button btn)
        {
            await btn.ScaleToAsync(0.9, 80, Easing.SpringIn);
            await btn.ScaleToAsync(1.0, 100, Easing.SpringOut);
        }
        
        _gameManager.StartNewGame();
    }
    
    protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_gameManager != null && _gameManager.IsRunning)
            {
                _gameManager.StopGame();
            }
        }
    
    private void InitializeBoard()
    {
        GameGridHelper.SetUpGrid(GameGrid, Rows, Cols);
        _cellButtons = new Button[Rows, Cols];

        var board = _gameManager.Board;
        
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                var cellModel = board[r, c];
                var cellButton = GameGridHelper.CreateCellButton(cellModel, OnCellTapped);

                _cellButtons[r, c] = cellButton;

                Grid.SetRow(cellButton, r);
                Grid.SetColumn(cellButton, c);
                GameGrid.Children.Add(cellButton);
            }
        }
    }
    
    private void UpdateScoreDisplay()
    {
        ScoreLabel.Text = _gameManager.CurrentPlayer.Score.ToString();
    }
    
    private void OnBoardUpdated(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                UpdateScoreDisplay();
                UpdateBoardColors();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Could not update grid: {ex.Message}");
            }
        });
    }

    private void OnCellTapped(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is GameCell tappedCell)
        {
            SoundHelper.PlayClickSound();
            
            _ = btn.ScaleToAsync(0.85, 50).ContinueWith(_ => btn.ScaleToAsync(1.0, 50));

            _gameManager.HandleCellTap(tappedCell);
        }
    }
    
    private void UpdateBoardColors()
    {
        var board =  _gameManager.Board;
        if (board == null || _cellButtons == null) return;

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                var cellModel = board[r, c];
                var button = _cellButtons[r, c];
                
                if (cellModel == null || button == null) continue;
                
                button.BackgroundColor = cellModel.IsCleared ? Colors.Transparent : cellModel.Color;
                
                if (cellModel.IsSelected)
                {
                    button.BorderColor = Colors.Gold;
                    button.BorderWidth = 3;
                }
                else button.BorderWidth = 0;
            }
        }
    }
}
