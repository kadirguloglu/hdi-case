using System.Diagnostics;

public static class WaitWatch
{
    public static async Task<int> WaitWatchAsync(int totalWaitMilisecond, Func<Task> action)
    {
        var watch = new Stopwatch();
        watch.Start();
        await action();
        watch.Stop();
        var totalMilisecond = Convert.ToInt32(watch.ElapsedMilliseconds);
        var diff = totalWaitMilisecond - totalMilisecond;
        return diff > 0 ? diff : 0;
    }
}