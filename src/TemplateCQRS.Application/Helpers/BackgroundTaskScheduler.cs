using System.Collections.Concurrent;

namespace TemplateCQRS.Application.Helpers;

public class BackgroundTaskScheduler
{
    private static BackgroundTaskScheduler _instance = null!;
    private readonly ConcurrentDictionary<string, Timer> _timers = new();

    private BackgroundTaskScheduler() { }

    public static BackgroundTaskScheduler Instance => _instance ??= new BackgroundTaskScheduler();

    /// <summary>
    ///     Schedules a task to run at the specified hour and minute, and then repeat at the specified interval.
    /// </summary>
    /// <param name="taskId">A unique identifier for the task.</param>
    /// <param name="task">The action to be executed when the task runs.</param>
    /// <param name="hour">The hour at which the task should first run. Default is 0.</param>
    /// <param name="min">The minute at which the task should first run. Default is 0.</param>
    /// <param name="intervalInHour">The number of hours between each run of the task. Default is 1.</param>
    /// <param name="intervalInMin">The number of minutes between each run of the task. Default is 0.</param>
    /// <param name="intervalInSeconds">The number of seconds between each run of the task. Default is 0.</param>
    public void ScheduleTask(string taskId, Action task, int hour = 0, int min = 0,
        double intervalInHour = 1, double intervalInMin = 0, double intervalInSeconds = 0)
    {
        if (_timers.ContainsKey(taskId)) return;

        var now = DateTime.Now;
        DateTime firstRun = new(now.Year, now.Month, now.Day, hour, min, 0, 0);

        if (now > firstRun)
        {
            Log.Logger.Debug($"Task {taskId} Begin");
            firstRun = firstRun.AddHours(intervalInHour);
            firstRun = firstRun.AddMinutes(intervalInMin);
            firstRun = firstRun.AddSeconds(intervalInSeconds);
        }

        var timeToGo = firstRun - now;
        if (timeToGo <= TimeSpan.Zero) timeToGo = TimeSpan.Zero;

        var interval = TimeSpan.FromHours(intervalInHour) + TimeSpan.FromMinutes(intervalInMin) +
                       TimeSpan.FromSeconds(intervalInSeconds);

        Timer timer = new(x =>
        {
            try
            {
                task.Invoke();
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"An error occurred in task {taskId}: {ex.Message}");
            }
        }, null, timeToGo, interval);

        _timers.TryAdd(taskId, timer);
    }

    /// <summary>
    ///     Removes all tasks from the scheduler.
    /// </summary>
    public void Clear() => _timers.Clear();

    /// <summary>
    ///     Removes a specific task from the scheduler.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task to remove.</param>
    /// <returns>A boolean indicating whether the task was successfully removed.
    ///     Returns true if the task was removed, false otherwise.</returns>
    public bool RemoveTask(string taskId) => _timers.TryRemove(taskId, out _);
}