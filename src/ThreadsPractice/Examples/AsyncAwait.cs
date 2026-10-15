using System.Diagnostics;

namespace ThreadsPractice.Examples;

/// <summary>
/// KONU: async / await
/// "İş bekler, işçi beklemez."
/// await: metot, task bitene kadar sonraki satıra geçmez, AMA thread'i kilitlemez.
///        Thread o sırada serbest kalır ve başka işlere bakar.
/// .Result / .Wait(): thread'i KİLİTLER (garson mutfak kapısında dikilir).
/// </summary>
public static class AsyncAwait
{
    public static async Task RunAsync()
    {
        // 1) await sonrası kod farklı bir thread'de devam edebilir
        Console.WriteLine($"Sipariş verildi.          [Thread {Environment.CurrentManagedThreadId}]");
        var yemek = await MutfakAsync("Kebap");
        Console.WriteLine($"{yemek} masaya götürüldü. [Thread {Environment.CurrentManagedThreadId}]");
        Console.WriteLine("NOT: Thread Id değişmiş olabilir. Yemek hazır olunca müsait olan bir garson devam etti.");

        // 2) await sırayla beklerken: 3 yemek x 1 saniye = ~3 saniye
        Console.WriteLine();
        var sw = Stopwatch.StartNew();
        await MutfakAsync("Çorba");
        await MutfakAsync("Pilav");
        await MutfakAsync("Tatlı");
        Console.WriteLine($"Sırayla await: {sw.ElapsedMilliseconds} ms");

        // 3) Hepsini aynı anda mutfağa verip birlikte beklersek: ~1 saniye
        sw.Restart();
        var sonuclar = await Task.WhenAll(
            MutfakAsync("Çorba"),
            MutfakAsync("Pilav"),
            MutfakAsync("Tatlı"));
        Console.WriteLine($"Task.WhenAll : {sw.ElapsedMilliseconds} ms -> {string.Join(", ", sonuclar)}");

        // 4) Kilitleyen (bloklayan) kullanım. Konsolda deadlock olmaz ama thread boşa bekler.
        //    UI uygulamalarında ve eski ASP.NET'te bu kullanım DEADLOCK'a yol açabilir.
        sw.Restart();
        var bloklu = MutfakAsync("Salata").Result; // ❌ yapma
        Console.WriteLine($".Result ile  : {sw.ElapsedMilliseconds} ms -> {bloklu} (bu sürede thread hiçbir şey yapamadı)");
    }

    // Async metot isimleri "Async" ile biter ve Task / Task<T> döner
    private static async Task<string> MutfakAsync(string yemek)
    {
        // Task.Delay = thread'i kilitlemeden beklemek (veritabanı veya API çağrısı gibi düşün)
        // Thread.Sleep olsaydı thread'i kilitlerdi
        await Task.Delay(1000);
        return yemek;
    }
}
