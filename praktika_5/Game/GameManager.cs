using praktika_5.Models;

namespace praktika_5.Game;

public class GameManager : Game
{
    public Cell[,] Board { get; private set; }
    private readonly GameboardService _gameboardService;
    private GameCell? _selectedCell;
    private readonly int _rows;
    private readonly int _columns;

    public event EventHandler? BoardUpdated;
    
    public GameManager(Player player, Theme initialTheme, int rows = 5, int columns = 5) : base(player, initialTheme)
    {
        _rows = rows;
        _columns = columns;
        _gameboardService = new GameboardService();
        IsRunning = false;
        Board = new Cell[_rows, _columns];
    }
    
    public void StartNewGame()
    {
        CurrentPlayer.ResetScore();
        var newBoard = _gameboardService.GenerateBoard(_rows, _columns);
        
        IsRunning = true;
        _selectedCell = null;
        
        BoardUpdated?.Invoke(this, EventArgs.Empty);
    }

    public bool HandleCellTap(GameCell tappedCell)
    {
        if (!IsRunning || tappedCell.IsCleared) return false;

        if (_selectedCell == null)
        {
            _selectedCell = tappedCell;
            _selectedCell.IsSelected = true;
            return true;
        }

        if (_selectedCell == tappedCell)
        {
            _selectedCell.IsSelected = false;
            _selectedCell = null;
            return true;
        }

        if (_gameboardService.CanConnect(_selectedCell, tappedCell))
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
            return true;
        }
        else
        {
            _selectedCell.IsSelected = false;
            _selectedCell = tappedCell;
            _selectedCell.IsSelected = true;
            return true;
        }
    }
    
    public void StopGame()
    {
        IsRunning = false;
    }
    
}