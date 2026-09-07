namespace example_app;

public partial class MainPage : ContentPage
{
	private int count = 0;
	// var primaryColor = (Color)Application.Current.Resources["Primary"];

	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object? sender, EventArgs e)
	{
		count++;
    
		if (count == 1)
			CounterBtn.Text = $"Vajutatud {count} kord";
		else
			CounterBtn.Text = $"Vajutatud {count} korda";

		if (count >= 5)
		{
			CounterBtn.BackgroundColor = Colors.Red;
			CounterBtn.TextColor = Colors.White;
		}
		
		BotImage.Rotation += 15; 

		CounterLabel.Text = $"Nuppu on vajutatud kokku: {count}";
		
		if (count >= 10)
		{
			BotImage.IsVisible = false;
			CounterLabel.Text = "Pilt kadus ära! Vajuta \"Alusta uuesti\".";
		}
		
		var random = new Random();
		var randomColor = Color.FromRgb(
			random.Next(0, 256), // Red
			random.Next(0, 256), // Green
			random.Next(0, 256)  // Blue
		);

		ResetBtn.BackgroundColor = randomColor;
    
		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private void OnResetClicked(object? sender, EventArgs e)
	{
		count = 0;
		CounterBtn.Text = "Vajuta mind";
		CounterLabel.Text = "Alusta uuesti";
		BotImage.Rotation = 0;
		BotImage.IsVisible = true;
		CounterBtn.BackgroundColor = (Color)Application.Current.Resources["Primary"];
		
		BotImage.HorizontalOptions = LayoutOptions.End;
	}
}
