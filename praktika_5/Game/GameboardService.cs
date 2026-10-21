using System.Text.RegularExpressions;
using praktika_5.Models;

namespace praktika_5.Game;

public class GameboardService
{
    private readonly List<Color> _availableColors;

    public GameboardService()
    {
        _availableColors = new List<Color>
        {
            Colors.Red,
            Colors.Blue,
            Colors.Green,
            Colors.Orange,
            Colors.Purple
        };
    }

    public GameCell[,] GenerateBoard(int rows, int columns)
    {
        Random random = new Random();
        GameCell[,] board = new GameCell[rows, columns];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Color randomColor = _availableColors[random.Next(_availableColors.Count)];
                board[r, c] = new GameCell(r, c, randomColor);
            }
        }
        return board;
    }

    public bool CanConnect(GameCell c1, GameCell c2, GameCell[,] board)
    {
        // Two cells must not be null, must be the same color, can not be one and the same cell and neither must be cleared.
        if (c1 == null || c2 == null || c1 == c2 || c1.Color != c2.Color || c1.IsCleared || c2.IsCleared) 
            return false;

        int rowDiff = c2.Row - c1.Row;
        int colDiff = c2.Column - c1.Column;

        int absRowDiff = Math.Abs(rowDiff);
        int absColDiff = Math.Abs(colDiff);
        
        // Check if the cells are on the same row or column
        bool isHorizontal = (absRowDiff == 0 && absColDiff > 0);
        bool isVertical = (absColDiff == 0 && absRowDiff > 0);

        if (!isHorizontal && !isVertical) return false;
        
        // Calculate steps direction
        int rowStep = absRowDiff == 0 ? 0 : rowDiff / absRowDiff;
        int colStep = absColDiff == 0 ? 0 : colDiff / absColDiff;

        int currentRow = c1.Row + rowStep;
        int currentCol = c1.Column + colStep;
        
        // Check if all cells between c1 and c2 are cleared
        while (currentRow != c2.Row || currentCol != c2.Column)
        {
            var intermediateCell = board[currentRow, currentCol];

            if (!intermediateCell.IsCleared)
            {
                return false;
            }

            currentRow += rowStep;
            currentCol += colStep;
        }
        
        return true;
    }
    
    public bool HasRemainingCells(GameCell[,] board)
    {
        foreach (var cell in board)
        {
            if (!cell.IsCleared) return true;
        }
        return false;
    }
}