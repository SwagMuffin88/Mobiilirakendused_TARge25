namespace praktika_3;

public partial class GamePage : ContentPage
{
    private HorizontalStackLayout _horizontalStackLayout;
    private VerticalStackLayout _verticalStackLayout;
    private Grid _gameBoard;
    
    
    
    public GamePage()
    {
        _gameBoard = CreateGameBoard();
        _verticalStackLayout = new VerticalStackLayout{ 
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { _gameBoard }
        };
        
        Content = _verticalStackLayout;
    }

    private Grid CreateGameBoard()
    {
        Grid grid = new Grid();
        for (int i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        return grid;
    }
}