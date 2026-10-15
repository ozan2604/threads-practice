namespace ThreadsPractice.Examples;

/// <summary>
/// KONU: Thread ile Task farkı
/// Thread = çalışan (her seferinde yeni garson işe almak, pahalı).
/// Task   = görev (hazır bekleyen garson ekibinden, yani ThreadPool'dan biri yapar).
/// </summary>
public static class ThreadVsTask
{
    public static async Task RunAsync()
    {
        // 1) Thread değer DÖNDÜREMEZ: sonucu dışarıdaki bir değişkene yazmak zorundayız
        int threadSonucu = 0;
        var thread = new Thread(() => threadSonucu = HesapYap());
        thread.Start();
        thread.Join();
        Console.WriteLine($"Thread sonucu: {threadSonucu} (dışarıdaki değişkene yazdık)");

        // 2) Task<T> değer DÖNDÜREBİLİR
        int taskSonucu = await Task.Run(() => HesapYap());
        Console.WriteLine($"Task sonucu  : {taskSonucu} (direkt geri döndü)");

        // 3) Task'taki hata await ile normal try/catch'e düşer
        try
        {
            await Task.Run(() => throw new InvalidOperationException("Mutfakta yangın var!"));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Task hatası yakalandı: {ex.Message}");
        }
        // Thread içinde fırlatılan hata ise dışarıdaki try/catch'e DÜŞMEZ, uygulamayı çökertir.
        // Bu yüzden onu burada çalıştırmıyoruz.

        // 4) ThreadPool: Task'lar hazır thread'leri tekrar kullanır
        Console.WriteLine();
        Console.WriteLine("10 Task çalıştırıyoruz. Thread Id'lerin tekrar ettiğine dikkat et (aynı garsonlar tekrar kullanılıyor):");
        var tasks = Enumerable.Range(1, 10).Select(i => Task.Run(() =>
        {
            Thread.Sleep(100);
            Console.WriteLine($"  Görev {i,2} -> Thread {Environment.CurrentManagedThreadId}");
        }));
        await Task.WhenAll(tasks);
    }

    private static int HesapYap()
    {
        Thread.Sleep(300);
        return 42;
    }
}
