# 🧵 Threads Practice (.NET 8)

A practice project that explains **Thread, Task, async/await, Multi-threading and Race Conditions** in C# using a restaurant analogy.

> 🍽️ **Analogy:** Program = restaurant, Thread = waiter, Task = a job given to a waiter, ThreadPool = a team of waiters on standby.

## 🚀 Running

```bash
dotnet run --project src/ThreadsPractice        # pick from the menu
dotnet run --project src/ThreadsPractice -- 5   # run example 5 directly
```

## 📚 Topics

| # | Topic | File | What it shows |
|---|-------|------|---------------|
| 1 | Thread Basics | [ThreadBasics.cs](src/ThreadsPractice/Examples/ThreadBasics.cs) | `new Thread`, `Start`, `Join`, thread Ids |
| 2 | Thread vs Task | [ThreadVsTask.cs](src/ThreadsPractice/Examples/ThreadVsTask.cs) | Returning values, catching exceptions, the ThreadPool reusing threads |
| 3 | async / await | [AsyncAwait.cs](src/ThreadsPractice/Examples/AsyncAwait.cs) | Waiting without blocking the thread, `Task.WhenAll`, the danger of `.Result` |
| 4 | Multi-thread | [MultiThreading.cs](src/ThreadsPractice/Examples/MultiThreading.cs) | Single thread vs multi-thread timing, `Parallel.For` |
| 5 | Race Condition | [RaceCondition.cs](src/ThreadsPractice/Examples/RaceCondition.cs) | Lost money, fixing it with `lock` and `Interlocked` |
| 6 | Bank Account | [BankAccount.cs](src/ThreadsPractice/Examples/BankAccount.cs) | A thread-safe `BankAccount` with `lock` |

## 📊 Sample Output (8-core machine)

```
Sequential await:  3014 ms
Task.WhenAll    :  1013 ms

Single thread:  3013 ms
Multi-thread :  1012 ms

No lock (register++):      220,125 $  ❌ $779,875 lost!
With lock           :    1,000,000 $  ✅ correct
With Interlocked    :    1,000,000 $  ✅ correct
```

## 🧠 Quick Summary

| Concept | In the restaurant | In one sentence |
|---|---|---|
| **Thread** | Waiter | The worker who does the job |
| **Task** | A job given to a waiter | Uses the ThreadPool, returns a value, exceptions can be caught |
| **async/await** | Checking other tables while the food cooks | **The job waits, the worker doesn't** |
| **Multi-thread** | More than one waiter | Jobs are done at the same time |
| **lock** | The cash register door | Only one thread gets in at a time |

## 🎯 Interview Questions

1. **What is the difference between a Thread and a Task?**
   A Thread is a worker, a Task is a job. A Task is more efficient because it uses the ready threads in the ThreadPool; it can return a value, and its exceptions can be caught with `await`.
2. **Does async/await create a new thread?**
   No. The method waits until the task finishes but doesn't block the thread; the thread handles other work in the meantime.
3. **Why are `.Result` / `.Wait()` dangerous?**
   They block the thread; in UI and legacy ASP.NET apps they can cause a deadlock.
4. **What is a race condition and how do you prevent it?**
   It's when the result gets corrupted because multiple threads change the same data at the same time. You prevent it with `lock`, `Interlocked` or thread-safe collections.
5. **What is the difference between multi-threading and async/await?**
   Multi-threading splits the work among more workers (CPU-bound). async/await keeps the same worker from waiting idly (I/O-bound).

## ✍️ Try It Yourself (Exercises)

- [ ] **RaceCondition:** Set `WaiterCount` to 1. Is money still lost in the no-lock version? Why?
- [ ] **AsyncAwait:** Replace the `Task.Delay` in `KitchenAsync` with `Thread.Sleep`. What changed?
- [ ] **MultiThreading:** Run 32 computations instead of 8 with `Parallel.For`. Did the time grow 4x? Think about how it relates to your core count.
- [x] **New example:** Write a `BankAccount` class with `Deposit` and `Withdraw` methods. Have 10 threads make transactions at the same time without corrupting the balance.
- [ ] **New example:** Call 5 different "API calls" (using `Task.Delay`) first one by one, then with `Task.WhenAll`, and compare the timings.

## 🔜 Next Topics

- Deadlock
- SemaphoreSlim (at most N operations at the same time)
- CancellationToken (cancelling work)
- Concurrent collections (`ConcurrentDictionary`, `ConcurrentQueue`)
