using praktika_5.Models;
using Cell = Microsoft.Maui.Controls.Cell;

namespace praktika_5.Game;

public class GameManager : Game
{
    public Cell[,] Board => _board;
    private readonly GameboardService _gameboardService;
    private Cell? _selectedCell;
    private readonly int _rows;
    private readonly int _columns;
    private Cell[,] _board;

    public event EventHandler? BoardUpdated;
    
    public GameManager(Player player, Theme initialTheme, int rows = 5, int columns = 5) : base(player, initialTheme)
    {
        _rows = rows;
        _columns = columns;
        _gameboardService = new GameboardService();
        _board = new Cell[rows, columns];
        IsRunning = false;
    }
}