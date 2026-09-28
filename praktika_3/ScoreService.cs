namespace praktika_3;

public class ScoreService
{
    public static int X_Wins { get; set; } = 0;
    public static int O_Wins { get; set; } = 0;
    public static int Draws { get; set; } = 0;
    
    public static void ResetScore()
    {
        X_Wins = 0;
        O_Wins = 0;
        Draws = 0;
    }
}