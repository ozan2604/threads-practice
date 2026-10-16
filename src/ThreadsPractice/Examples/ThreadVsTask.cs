namespace ThreadsPractice.Examples;

/// <summary>
/// TOPIC: The difference between Thread and Task
/// Thread = a worker (hiring a new waiter every time, expensive).
/// Task   = a job (done by one of the waiters already on standby, i.e. the ThreadPool).
/// </summary>
public static class ThreadVsTask
{
    public static async Task RunAsync()
    {
        // 1) A Thread CANNOT return a value: we have to write the result to an outside variable
        int threadResult = 0;
        var thread = new Thread(() => threadResult = Compute());
        thread.Start();
        thread.Join();
        Console.WriteLine($"Thread result: {threadResult} (written to an outside variable)");

        // 2) A Task<T> CAN return a value
        int taskResult = await Task.Run(() => Compute());
        Console.WriteLine($"Task result  : {taskResult} (returned directly)");

        // 3) An exception in a Task lands in a normal try/catch through await
        try
        {
            await Task.Run(() => throw new InvalidOperationException("The kitchen is on fire!"));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Task exception caught: {ex.Message}");
        }
        // An exception thrown inside a Thread, however, does NOT reach the outer try/catch; it crashes the app.
        // That's why we don't run it here.

        // 4) ThreadPool: Tasks reuse existing threads
        Console.WriteLine();
        Console.WriteLine("Running 10 Tasks. Notice how the Thread Ids repeat (the same waiters are being reused):");
        var tasks = Enumerable.Range(1, 10).Select(i => Task.Run(() =>
        {
            Thread.Sleep(100);
            Console.WriteLine($"  Job {i,2} -> Thread {Environment.CurrentManagedThreadId}");
        }));
        await Task.WhenAll(tasks);
    }

    private static int Compute()
    {
        Thread.Sleep(300);
        return 42;
    }
}
