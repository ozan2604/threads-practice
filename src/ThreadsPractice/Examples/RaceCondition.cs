namespace ThreadsPractice.Examples;

/// <summary>
/// KONU: Race Condition ve lock
/// Birden fazla thread aynı veriyi aynı anda değiştirirse sonuç bozulur.
/// Çözüm: lock (kasanın önüne kapı koymak, içeride biri varsa diğeri bekler).
/// </summary>
public static class RaceCondition
{
    private const int GarsonSayisi = 10;
    private const int HerGarsonunEklemeSayisi = 100_000;
    private const int Beklenen = GarsonSayisi * HerGarsonunEklemeSayisi;

    private static readonly object _kilit = new();

    public static async Task RunAsync()
    {
        Console.WriteLine($"{GarsonSayisi} garson, her biri kasaya {HerGarsonunEklemeSayisi:N0} kez 1 TL ekliyor.");
        Console.WriteLine($"Beklenen kasa: {Beklenen:N0} TL");
        Console.WriteLine();

        // 1) ❌ Kilitsiz: race condition
        // kasa++ aslında 3 adımdır: OKU -> 1 EKLE -> YAZ
        // İki garson aynı anda okursa birinin eklediği para kaybolur.
        int kasa = 0;
        await GarsonlariCalistir(() => kasa++);
        Yazdir("Kilitsiz (kasa++)", kasa);

        // 2) ✅ lock: aynı anda sadece bir garson kasaya dokunabilir
        kasa = 0;
        await GarsonlariCalistir(() =>
        {
            lock (_kilit)
            {
                kasa++;
            }
        });
        Yazdir("lock ile", kasa);

        // 3) ✅ Interlocked: basit sayısal işlemler için tek adımda (atomik) artırma, lock'tan hızlıdır
        kasa = 0;
        await GarsonlariCalistir(() => Interlocked.Increment(ref kasa));
        Yazdir("Interlocked ile", kasa);
    }

    private static Task GarsonlariCalistir(Action paraEkle)
    {
        var garsonlar = Enumerable.Range(0, GarsonSayisi).Select(_ => Task.Run(() =>
        {
            for (int i = 0; i < HerGarsonunEklemeSayisi; i++)
                paraEkle();
        }));
        return Task.WhenAll(garsonlar);
    }

    private static void Yazdir(string baslik, int kasa)
    {
        var durum = kasa == Beklenen ? "✅ doğru" : $"❌ {Beklenen - kasa:N0} TL kayboldu!";
        Console.WriteLine($"{baslik,-20}: {kasa,12:N0} TL  {durum}");
    }
}
