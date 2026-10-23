using praktika_5.Models;

namespace praktika_5.Game;

public static class AppThemes
{
    public static Theme Light => new Theme(
        "Hele", 
        Colors.White, 
        Colors.Black, 
        Colors.RoyalBlue, 
        Colors.White, 
        "Roboto-Regular"
    );
    
    public static Theme Dark => new Theme(
        "Tume", 
        Color.FromArgb("#121212"), 
        Colors.White, 
        Color.FromArgb("#BB86FC"), 
        Color.FromArgb("#1E1E1E"), 
        "Montserrat-Regular"
    );
    
    public static Theme Colorful => new Theme(
        "Värviline", 
        Color.FromArgb("#FFF8E1"), 
        Color.FromArgb("#3E2723"), 
        Color.FromArgb("#FF8F00"), 
        Color.FromArgb("#FFE082"), 
        "Nunito-Regular"
    );
}