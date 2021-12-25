using BeProductive.Modules.Common.Infrastructure;
using BeProductive.Modules.Utils.FluentScheduler;

namespace BeProductive.Modules.Timer.Infrastructure; 

public class TimerService : IDisposable {
    private AudioService _audioService;
    
    private DateTime _startTime;
    private DateTime _endTime;
    private Job? _timerFinishedJob;
    private Job? _timerTickJob;

    public TimerService(AudioService audioService)
    {
        _audioService = audioService;
    }

    public bool IsRunning { get; private set; }

    public event EventHandler? TimerTick;
    public event EventHandler? TimerFinished;

    public void Start(TimeSpan duration)
    {
        IsRunning = true;
        _startTime = DateTime.Now;
        _endTime = _startTime + duration;
        ScheduleElapsedTask();
    }

    public void Stop()
    {
        IsRunning = false;
        StopJobs();
    }

    private void ScheduleElapsedTask()
    {
        var timeLeft = _endTime - DateTime.UtcNow;

        _timerTickJob = JobHelper.AddJob(OnTimerTick, s => {
            s.ToRunEvery(1).Seconds();
        });
        _timerFinishedJob = JobHelper.AddJob(OnTimerFinished, s => s.ToRunOnceAt(_endTime));
    }

    private void OnTimerTick()
    {
        TimerTick?.Invoke(this, EventArgs.Empty);
    }
    
    private void OnTimerFinished()
    {
        TimerFinished?.Invoke(this, EventArgs.Empty);
        Stop();
        _ = _audioService.PlaySoundEffect(SoundEffect.TimerFinish);
    }

    private void StopJobs()
    {
        _timerFinishedJob?.Stop();
        _timerTickJob?.Stop();
    }

    public void Dispose()
    {
        StopJobs();
    }

}
