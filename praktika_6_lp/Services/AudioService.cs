using Plugin.Maui.Audio;

namespace praktika_6.Services;

public class AudioService
{
    public async Task PLayClickSound()
    {
        var audioPlayer = AudioManager.Current.CreatePlayer(
            await FileSystem.OpenAppPackageFileAsync("click.mp3"));
        
        audioPlayer.Play();
    }
}