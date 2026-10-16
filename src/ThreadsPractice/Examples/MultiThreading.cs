using System.Diagnostics;

namespace ThreadsPractice.Examples;

/// <summary>
/// TOPIC: Multi-thread
/// One waiter (single-thread): jobs are done one after another.
/// Many waiters (multi-thread): jobs are done at the same time and finish faster.
/// </summary>
public static class MultiThreading
{
    public static async Task RunAsync()
    {
        Console.WriteLine($"Number of cores on your machine: {Environment.ProcessorCount}");

        // 1) One waiter: 3 tables x 1 second = ~3 seconds
        var sw = Stopwatch.StartNew();
        ServeTable(1);
        ServeTable(2);
        ServeTable(3);
        Console.WriteLine($"Single thread: {sw.ElapsedMilliseconds} ms");
        Console.WriteLine();

        // 2) Many waiters: 3 tables at the same time = ~1 second
        sw.Restart();
        var t1 = Task.Run(() => ServeTable(1));
        var t2 = Task.Run(() => ServeTable(2));
        var t3 = Task.Run(() => ServeTable(3));
        await Task.WhenAll(t1, t2, t3); // wait for all three to finish
        Console.WriteLine($"Multi-thread : {sw.ElapsedMilliseconds} ms");
        Console.WriteLine();

        // 3) Parallel.For: the easy way to split heavy computation (CPU-bound) work
        sw.Restart();
        for (int i = 0; i < 8; i++) HeavyComputation();
        var sequential = sw.ElapsedMilliseconds;

        sw.Restart();
        Parallel.For(0, 8, _ => HeavyComputation());
        var parallel = sw.ElapsedMilliseconds;

        Console.WriteLine($"8 heavy computations, for         : {sequential} ms");
        Console.WriteLine($"8 heavy computations, Parallel.For: {parallel} ms");
    }

    private static void ServeTable(int table)
    {
        Thread.Sleep(1000); // the job takes 1 second
        Console.WriteLine($"  Table {table} done [Thread {Environment.CurrentManagedThreadId}]");
    }

    // A job that actually works the CPU (not Sleep)
    private static double HeavyComputation()
    {
        double total = 0;
        for (int i = 1; i < 20_000_000; i++)
            total += Math.Sqrt(i);
        return total;
    }
}
