using Microsoft.JSInterop;

namespace BeProductive.Modules.Common.Infrastructure;

public class AudioService
{
    private IJSRuntime _jsRuntime;

    public AudioService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task PlaySoundEffect(SoundEffect soundEffect)
    {
        var url = soundEffect switch
        {
            SoundEffect.SelectGoalSuccess => "/sounds/state-change_confirm-up.ogg",
            SoundEffect.SelectGoalFailure => "/sounds/Ding-Error-Pitch-Drop-01.wav",
            SoundEffect.SelectGoalUncheck => "/sounds/Pop-High-Round-Short-01.wav",
            SoundEffect.SelectGoalEmergency => "/sounds/state-change_confirm-down.ogg",
            SoundEffect.Select or SoundEffect.Cancel => "/sounds/navigation-cancel.ogg",
            _ => throw new ArgumentOutOfRangeException(nameof(soundEffect), soundEffect,
                $"No sound effect path for {soundEffect}")
        };

        await PlayUrl(url);
    }

    public async Task PlayUrl(string url)
    {
        await _jsRuntime.InvokeAsync<object>("PlayAudio", url);
    }
}