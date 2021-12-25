using Quartz;
using Serilog;

namespace BeProductive.Modules.Timer.Infrastructure; 

public class TimerService {
    private readonly ISchedulerFactory _schedulerFactory;
    
    private DateTime _startTime;
    private DateTime _endTime;
    
    public bool IsRunning { get; private set; }

    public event EventHandler TimerFinished;
    
    public TimerService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Start(TimeSpan duration)
    {
        IsRunning = true;
        _startTime = DateTime.UtcNow;
        _endTime = _startTime + duration;
        await ScheduleElapsedTask();
    }

    public void Stop()
    {
        IsRunning = false;
    }

    private async Task ScheduleElapsedTask()
    {
        var timeLeft = _endTime - DateTime.UtcNow;

        var job = JobBuilder.Create<TimerJob>()
            .Build();
        
        var trigger = TriggerBuilder.Create()
            .StartAt(_endTime)
            .Build();
        
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.ScheduleJob(job, trigger);
    }

    private void OnTimerFinished()
    {
        
    }
}

public class TimerJob : IJob {

    private readonly TimerService _timerService;

    public async Task Execute(IJobExecutionContext context)
    {
        Log.Warning("Timer fired!");
    }
}