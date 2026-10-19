namespace praktika_5.Models;

public class Cell
{
    public int Row { get; private set; }
    public int Column { get; private set; }
    public Color Color { get; set; }
    public bool IsSelected { get; set; }
    public bool IsCleared { get; set; }
    
    public Cell(int row, int column,  Color color)
    {
        Row = row;
        Column = column;
        Color = color;
        IsSelected = false;
        IsCleared = false;
    }
}