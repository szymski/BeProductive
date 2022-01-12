using BeProductive.Modules.Settings.Infrastructure;
using Microsoft.JSInterop;

namespace BeProductive.Modules.Common.Infrastructure;

public class AudioService
{
    private IJSRuntime _jsRuntime;
    private SettingsService _settingsService;

    public AudioService(
        IJSRuntime jsRuntime,
        SettingsService settingsService)
    {
        _jsRuntime = jsRuntime;
        _settingsService = settingsService;
    }

    public async Task PlaySoundEffect(SoundEffect soundEffect)
    {
        var url = soundEffect switch
        {
            SoundEffect.SelectGoalSuccess => "/sounds/state-change_confirm-up.ogg",
            SoundEffect.SelectGoalFailure => "/sounds/Ding-Error-Pitch-Drop-01.wav",
            SoundEffect.SelectGoalUncheck => "/sounds/Pop-High-Round-Short-01.wav",
            SoundEffect.SelectGoalEmergency => "/sounds/state-change_confirm-down.ogg",
            SoundEffect.SelectGoalNotApplicable => "/sounds/timer/pause.wav",
            SoundEffect.Select or SoundEffect.Cancel => "/sounds/navigation-cancel.ogg",
            SoundEffect.TimerStart => "/sounds/timer/start.wav",
            SoundEffect.TimerPause => "/sounds/timer/pause.wav",
            SoundEffect.TimerResume => "/sounds/timer/resume.wav",
            SoundEffect.TimerFinish => "/sounds/timer/finish.wav",
            _ => throw new ArgumentOutOfRangeException(nameof(soundEffect), soundEffect,
                $"No sound effect path for {soundEffect}")
        };

        await PlayUrl(url);
    }

    public async Task PlayUrl(string url)
    {
        if (!_settingsService.SoundsEnabled) return;
        await _jsRuntime.InvokeAsync<object>("PlayAudio", url);
    }
}