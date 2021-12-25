using FluentScheduler;

namespace BeProductive.Modules.Utils.FluentScheduler;

public static class JobHelper {
    public static string RandomName => Guid.NewGuid().ToString();

    public static Job AddJob(Action job, Action<Schedule> schedule)
    {
        var name = RandomName;
        JobManager.AddJob(job, s => {
            schedule(s.WithName(name));
        });
        return new(name);
    }
}

public class Job {
    private readonly string _name;

    public Job(string name)
    {
        _name = name;
    }

    public void Stop()
    {
        JobManager.RemoveJob(_name);
    }
}