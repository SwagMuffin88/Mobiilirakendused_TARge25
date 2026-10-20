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
}