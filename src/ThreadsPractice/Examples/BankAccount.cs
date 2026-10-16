namespace ThreadsPractice.Examples;

/// <summary>
/// EXERCISE: Thread-safe bank account
/// Multiple customers (threads) deposit and withdraw money at the same time.
/// Thanks to lock, the balance never gets corrupted.
/// </summary>
public class BankAccount
{
    private decimal _balance;
    private readonly object _lock = new();

    public BankAccount(decimal initialBalance)
    {
        _balance = initialBalance;
    }

    // We lock while reading too: decimal is 128 bits, so reading it is not a single-step operation.
    // Without the lock, we could read a half-written value while another thread is writing.
    public decimal Balance
    {
        get
        {
            lock (_lock)
            {
                return _balance;
            }
        }
    }

    public void Deposit(decimal amount)
    {
        lock (_lock)
        {
            _balance += amount;
        }
    }

    public bool Withdraw(decimal amount)
    {
        // The check and the subtraction must be inside the SAME lock.
        // If they were separate, two customers could both see "balance is enough"
        // at the same time, both withdraw, and the balance would go negative.
        lock (_lock)
        {
            if (_balance < amount)
                return false;

            _balance -= amount;
            return true;
        }
    }
}

public static class BankAccountExample
{
    private const int CustomerCount = 10;
    private const int TransactionCount = 1000;

    public static async Task RunAsync()
    {
        var account = new BankAccount(100_000);

        // 10 customers are making transactions at the same time.
        // Each customer deposits $10 1000 times and withdraws $5 1000 times.
        // Expected balance: 100,000 + 100,000 - 50,000 = $150,000
        var customers = Enumerable.Range(1, CustomerCount).Select(_ => Task.Run(() =>
        {
            for (int i = 0; i < TransactionCount; i++)
            {
                account.Deposit(10);
                account.Withdraw(5);
            }
        }));

        // Wait until all customers are done
        await Task.WhenAll(customers);

        const decimal expected = 150_000;
        var status = account.Balance == expected ? "✅ correct" : "❌ balance corrupted!";
        Console.WriteLine($"Final balance: ${account.Balance:N0} (expected: ${expected:N0})  {status}");
    }
}
