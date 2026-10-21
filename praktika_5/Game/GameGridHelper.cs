using praktika_5.Models;

namespace praktika_5.Game;

public class GameGridHelper
{
    
    public static void SetUpGrid(Grid grid, int rows, int cols)
    {
        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        for (int r = 0; r < rows; r++)
        {
            grid.RowDefinitions.Add(
                new RowDefinition(
                    GridLength.Auto
                )
            );
        }

        for (int c = 0; c < cols; c++)
        {
            grid.ColumnDefinitions.Add(
                new ColumnDefinition(GridLength.Auto
                )
            );
        }
    }
    
    public static Button CreateCellButton(GameCell cellModel, EventHandler clickHandler)
    {
        var button = new Button
        {
            WidthRequest = 50,
            HeightRequest = 50,
            BackgroundColor = cellModel.IsCleared ? Colors.Transparent : cellModel.Color,
            CornerRadius = 8
        };
        
        button.CommandParameter = cellModel;
        
        if (clickHandler != null)
        {
            button.Clicked += clickHandler;
        }

        return button;
    }
}