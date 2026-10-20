using praktika_5.Models;
using Cell = Microsoft.Maui.Controls.Cell;

namespace praktika_5.Game;

public class Game
{
    public Player CurrentPlayer { get; protected set; }
    public Theme CurrentTheme { get; protected set; }
    public bool IsRunning { get; protected set; }

    public Game(Player player, Theme initialTheme)
    {
        CurrentPlayer = player;
        CurrentTheme = initialTheme;
        IsRunning = false;
    }
    
    public virtual void Start()
    {
        IsRunning = true;
        CurrentPlayer.ResetScore();
    }
    
    public virtual void Stop()
    {
        IsRunning = false;
    }
    
    public void ChangeTheme(Theme newTheme, ContentPage page)
    {
        if (newTheme == null) return;
        CurrentTheme = newTheme;
        CurrentTheme.Apply(page);
    }
}