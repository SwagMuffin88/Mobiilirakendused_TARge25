using praktika_5.Game;
using praktika_5.Models;

namespace praktika_5;

public partial class MainPage : ContentPage
{
    private GameManager _gameManager;
    private Theme _currentTheme;
    
	public MainPage()
	{
		InitializeComponent();

        var player = new Player("Mängija 1");
        PlayerNameLabel.Text = player.Name;

        // Default theme: light
        _currentTheme = new Theme(
            "Hele",
            Colors.White,
            Colors.Black, 
            Colors.RoyalBlue,
            Colors.LightGray,
            "OpenSansRegular");
        _currentTheme.Apply(this);
        
        _gameManager = new GameManager(player, _currentTheme, 5, 5);
        _gameManager.BoardUpdated += OnBoardUpdated;
        
        _gameManager.StartNewGame();
    }
    
    private void OnNewGameClicked(object sender, EventArgs e)
    {
        _gameManager.StartNewGame();
    }

	private void OnBoardUpdated(object? sender, EventArgs e)
	{
        
	}

    private async void OnCellTapped(GameCell tappedCell)
    {
        bool actionTaken = _gameManager.HandleCellTap(tappedCell);

        if (actionTaken)
        {
            if(!_gameManager.IsRunning) await DisplayAlertAsync(
                "Mäng läbi!", $"Tubli! Sinu tulemus: {_gameManager.CurrentPlayer.Score}", "OK");
        }
    }
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (_gameManager != null && _gameManager.IsRunning)
        {
            _gameManager.StopGame();
        }
    }
}
