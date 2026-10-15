using System.Diagnostics;

namespace ThreadsPractice.Examples;

/// <summary>
/// KONU: Multi-thread
/// Tek garson (single-thread): işler sırayla yapılır.
/// Çok garson (multi-thread): işler aynı anda yapılır, daha hızlı biter.
/// </summary>
public static class MultiThreading
{
    public static async Task RunAsync()
    {
        Console.WriteLine($"Bilgisayarındaki çekirdek sayısı: {Environment.ProcessorCount}");

        // 1) Tek garson: 3 masa x 1 saniye = ~3 saniye
        var sw = Stopwatch.StartNew();
        MasayaBak(1);
        MasayaBak(2);
        MasayaBak(3);
        Console.WriteLine($"Tek thread  : {sw.ElapsedMilliseconds} ms");
        Console.WriteLine();

        // 2) Çok garson: 3 masa aynı anda = ~1 saniye
        sw.Restart();
        var t1 = Task.Run(() => MasayaBak(1));
        var t2 = Task.Run(() => MasayaBak(2));
        var t3 = Task.Run(() => MasayaBak(3));
        await Task.WhenAll(t1, t2, t3); // üçünün de bitmesini bekle
        Console.WriteLine($"Multi-thread: {sw.ElapsedMilliseconds} ms");
        Console.WriteLine();

        // 3) Parallel.For: ağır hesaplama (CPU-bound) işleri bölüştürmenin kolay yolu
        sw.Restart();
        for (int i = 0; i < 8; i++) AgirHesap();
        var sirali = sw.ElapsedMilliseconds;

        sw.Restart();
        Parallel.For(0, 8, _ => AgirHesap());
        var paralel = sw.ElapsedMilliseconds;

        Console.WriteLine($"8 ağır hesap, for         : {sirali} ms");
        Console.WriteLine($"8 ağır hesap, Parallel.For: {paralel} ms");
    }

    private static void MasayaBak(int masa)
    {
        Thread.Sleep(1000); // iş 1 saniye sürüyor
        Console.WriteLine($"  Masa {masa} tamam [Thread {Environment.CurrentManagedThreadId}]");
    }

    // Gerçekten CPU'yu yoran bir iş (Sleep değil)
    private static double AgirHesap()
    {
        double toplam = 0;
        for (int i = 1; i < 20_000_000; i++)
            toplam += Math.Sqrt(i);
        return toplam;
    }
}
