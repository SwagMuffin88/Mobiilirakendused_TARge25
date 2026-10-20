namespace praktika_5.Models;

public class Player
{
    private string _name;
    private int _score;
    private int _highScore;

    public string Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value) ? "Mängija" : value.Trim();
    }

    public int Score
    {
        get => _score;
        set => _score = Math.Max(0, value);
    }
    
    public int HighScore
    {
        get => _highScore;
        set => _highScore = Math.Max(0, value);
    }

    public Player(string name)
    {
        Name = name;
        _score = 0;
        _highScore = 0;
    }
    
    public void ResetScore()
    {
        Score = 0;
    }
}