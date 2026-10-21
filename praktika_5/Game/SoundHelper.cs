using Plugin.Maui.Audio;

namespace praktika_5.Game;

public static class SoundHelper
{
    public static async void PlayClickSound()
    {
        await PlaySoundFileAsync("click.mp3");
    }

    public static async void PlayClearSound()
    {
        await PlaySoundFileAsync("clear.mp3");
    }

    private static async Task PlaySoundFileAsync(string fileName)
    {
        try
        {
            var audioStream = await FileSystem.OpenAppPackageFileAsync(fileName);
            var player = AudioManager.Current.CreatePlayer(audioStream);
            player.Play();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Could not play sound ({fileName}): {e.Message}");
        }
    }
}