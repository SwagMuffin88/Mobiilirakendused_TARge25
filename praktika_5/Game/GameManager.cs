using praktika_5.Models;

namespace praktika_5.Game;

public class GameManager : Game
{
    public GameCell[,] Board { get; private set; }
    private readonly GameboardService _gameboardService;
    private GameCell? _selectedCell;
    private readonly int _rows;
    private readonly int _columns;

    public event EventHandler? BoardUpdated;
    public event EventHandler? GameOver;
    
    public GameManager(Player player, Theme initialTheme, int rows, int columns) : base(player, initialTheme)
    {
        _rows = rows;
        _columns = columns;
        _gameboardService = new GameboardService();
        IsRunning = false;
        Board = new GameCell[_rows, _columns];
    }
    
    public void StartNewGame()
    {
        CurrentPlayer.ResetScore();
        Board = _gameboardService.GenerateBoard(_rows, _columns);
        
        IsRunning = true;
        _selectedCell = null;
        
        BoardUpdated?.Invoke(this, EventArgs.Empty);
    }

    public bool HandleCellTap(GameCell tappedCell)
    {
        if (!IsRunning || tappedCell.IsCleared) return false;
        
        bool stateChanged = false;
        
        if (_selectedCell == null)
        {
            _selectedCell = tappedCell;
            _selectedCell.IsSelected = true;
            stateChanged = true;
        }
        else if (_selectedCell == tappedCell)
        {
            _selectedCell.IsSelected = false;
            _selectedCell = null;
            stateChanged = true;
        }
        else if (_gameboardService.CanConnect(_selectedCell, tappedCell, Board))
        {
            ClearConnectedCells(tappedCell);
            stateChanged = true;
        }
        else
        {
            _selectedCell.IsSelected = false;
            _selectedCell = tappedCell;
            _selectedCell.IsSelected = true;
            stateChanged = true;
        }
        
        if (stateChanged)
        {
            BoardUpdated?.Invoke(this, EventArgs.Empty);
        }

        return stateChanged;
    }

    private void ClearConnectedCells(GameCell tappedCell)
    {
        _selectedCell.IsCleared = true;
        _selectedCell.IsSelected = false;

        tappedCell.IsCleared = true;
        tappedCell.IsSelected = false;

        CurrentPlayer.Score += 100;
            
        if (CurrentPlayer.Score > CurrentPlayer.HighScore)
        {
            CurrentPlayer.HighScore = CurrentPlayer.Score;
        }

        _selectedCell = null;

        if (!_gameboardService.HasAvailablePairs(Board) || !_gameboardService.HasRemainingCells(Board))
        {
            StopGame();
            GameOver?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void StopGame()
    {
        IsRunning = false;
    }
}