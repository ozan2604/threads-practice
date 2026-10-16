namespace ThreadsPractice.Examples;

/// <summary>
/// TOPIC: Race Condition and lock
/// If multiple threads change the same data at the same time, the result gets corrupted.
/// Solution: lock (put a door in front of the cash register; if someone is inside, the others wait).
/// </summary>
public static class RaceCondition
{
    private const int WaiterCount = 10;
    private const int AdditionsPerWaiter = 100_000;
    private const int Expected = WaiterCount * AdditionsPerWaiter;

    private static readonly object _lock = new();

    public static async Task RunAsync()
    {
        Console.WriteLine($"{WaiterCount} waiters, each adding $1 to the register {AdditionsPerWaiter:N0} times.");
        Console.WriteLine($"Expected register total: ${Expected:N0}");
        Console.WriteLine();

        // 1) ❌ Without lock: race condition
        // register++ is actually 3 steps: READ -> ADD 1 -> WRITE
        // If two waiters read at the same time, the money one of them added is lost.
        int register = 0;
        await RunWaiters(() => register++);
        Print("No lock (register++)", register);

        // 2) ✅ lock: only one waiter can touch the register at a time
        register = 0;
        await RunWaiters(() =>
        {
            lock (_lock)
            {
                register++;
            }
        });
        Print("With lock", register);

        // 3) ✅ Interlocked: single-step (atomic) increment for simple numeric operations, faster than lock
        register = 0;
        await RunWaiters(() => Interlocked.Increment(ref register));
        Print("With Interlocked", register);
    }

    private static Task RunWaiters(Action addMoney)
    {
        var waiters = Enumerable.Range(0, WaiterCount).Select(_ => Task.Run(() =>
        {
            for (int i = 0; i < AdditionsPerWaiter; i++)
                addMoney();
        }));
        return Task.WhenAll(waiters);
    }

    private static void Print(string title, int register)
    {
        var status = register == Expected ? "✅ correct" : $"❌ ${Expected - register:N0} lost!";
        Console.WriteLine($"{title,-20}: {register,12:N0} $  {status}");
    }
}
