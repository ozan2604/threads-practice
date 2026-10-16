namespace ThreadsPractice.Examples;

/// <summary>
/// TOPIC: What is a thread?
/// Thread = the worker who does the job (a waiter).
/// The Main method also runs on a thread (the Main thread).
/// </summary>
public static class ThreadBasics
{
    public static Task RunAsync()
    {
        Console.WriteLine($"Main thread Id: {Environment.CurrentManagedThreadId}");

        // We create a new waiter (thread) and give it a job
        var waiter1 = new Thread(() => TakeOrder("Table 1"));
        var waiter2 = new Thread(() => TakeOrder("Table 2"));

        // A thread doesn't run until you call Start()
        waiter1.Start();
        waiter2.Start();

        // Join() = "wait until this waiter finishes the job"
        // Without Join, the Main thread would continue without waiting for the waiters
        waiter1.Join();
        waiter2.Join();

        Console.WriteLine("Both waiters finished their jobs.");
        Console.WriteLine("NOTE: The order of the output may change on every run, because threads run at the same time.");

        return Task.CompletedTask;
    }

    private static void TakeOrder(string table)
    {
        // Every thread has its own Id: this is how we see they are different waiters
        Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] Taking the order for {table}...");
        Thread.Sleep(500); // the job takes 0.5 seconds
        Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] Order for {table} done.");
    }
}
