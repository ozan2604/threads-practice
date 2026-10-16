using System.Diagnostics;

namespace ThreadsPractice.Examples;

/// <summary>
/// TOPIC: async / await
/// "The job waits, the worker doesn't."
/// await: the method doesn't move to the next line until the task finishes, BUT it doesn't block the thread.
///        The thread is freed in the meantime and handles other work.
/// .Result / .Wait(): BLOCKS the thread (the waiter just stands at the kitchen door).
/// </summary>
public static class AsyncAwait
{
    public static async Task RunAsync()
    {
        // 1) Code after await may continue on a different thread
        Console.WriteLine($"Order placed.             [Thread {Environment.CurrentManagedThreadId}]");
        var dish = await KitchenAsync("Kebab");
        Console.WriteLine($"{dish} served to the table. [Thread {Environment.CurrentManagedThreadId}]");
        Console.WriteLine("NOTE: The Thread Id may have changed. When the food was ready, whichever waiter was free took over.");

        // 2) Awaiting one by one: 3 dishes x 1 second = ~3 seconds
        Console.WriteLine();
        var sw = Stopwatch.StartNew();
        await KitchenAsync("Soup");
        await KitchenAsync("Rice");
        await KitchenAsync("Dessert");
        Console.WriteLine($"Sequential await: {sw.ElapsedMilliseconds} ms");

        // 3) If we send them all to the kitchen at once and wait together: ~1 second
        sw.Restart();
        var results = await Task.WhenAll(
            KitchenAsync("Soup"),
            KitchenAsync("Rice"),
            KitchenAsync("Dessert"));
        Console.WriteLine($"Task.WhenAll    : {sw.ElapsedMilliseconds} ms -> {string.Join(", ", results)}");

        // 4) Blocking usage. No deadlock in a console app, but the thread waits for nothing.
        //    In UI apps and legacy ASP.NET this usage can cause a DEADLOCK.
        sw.Restart();
        var blocked = KitchenAsync("Salad").Result; // ❌ don't do this
        Console.WriteLine($"With .Result    : {sw.ElapsedMilliseconds} ms -> {blocked} (the thread couldn't do anything during this time)");
    }

    // Async method names end with "Async" and return Task / Task<T>
    private static async Task<string> KitchenAsync(string dish)
    {
        // Task.Delay = waiting without blocking the thread (think of a database or API call)
        // Thread.Sleep would block the thread
        await Task.Delay(1000);
        return dish;
    }
}
