namespace praktika_5.Models;

public class GameCell
{
    public int Row { get;  set; }
    public int Column { get;  set; }
    public Color Color { get; set; }
    public bool IsSelected { get; set; }
    public bool IsCleared { get; set; }
    
    public GameCell(int row, int column,  Color color)
    {
        Row = row;
        Column = column;
        Color = color;
        IsSelected = false;
        IsCleared = false;
    }
}