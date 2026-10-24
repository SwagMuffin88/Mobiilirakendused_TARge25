using Plugin.Maui.Audio;

namespace praktika_6.Services;

public class AudioService
{
    public async Task PlayClickSound()
    {
        var audioPlayer = AudioManager.Current.CreatePlayer(
            await FileSystem.OpenAppPackageFileAsync("bubble.mp3"));
        
        audioPlayer.Play();
    }
}