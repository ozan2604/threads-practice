namespace ThreadsPractice.Examples;

/// <summary>
/// ALIŞTIRMA: Thread-safe banka hesabı
/// Birden fazla müşteri (thread) aynı anda para yatırıp çekiyor.
/// lock sayesinde bakiye bozulmuyor.
/// </summary>
public class BankaHesabi
{
    private decimal _bakiye;
    private readonly object _kilit = new();

    public BankaHesabi(decimal baslangicBakiyesi)
    {
        _bakiye = baslangicBakiyesi;
    }

    // Okurken de kilitliyoruz: decimal 128 bit olduğu için okuma işlemi tek adımda yapılmaz.
    // Kilitlemezsek, başka bir thread yazarken yarım kalmış bir değer okuyabiliriz.
    public decimal Bakiye
    {
        get
        {
            lock (_kilit)
            {
                return _bakiye;
            }
        }
    }

    public void ParaYatir(decimal miktar)
    {
        lock (_kilit)
        {
            _bakiye += miktar;
        }
    }

    public bool ParaCek(decimal miktar)
    {
        // Kontrol ve düşme işlemi AYNI lock içinde olmalı.
        // Ayrı olsaydı iki müşteri aynı anda "bakiye yeterli" görüp
        // ikisi birden çekebilirdi ve bakiye eksiye düşerdi.
        lock (_kilit)
        {
            if (_bakiye < miktar)
                return false;

            _bakiye -= miktar;
            return true;
        }
    }
}

public static class BankaHesabiOrnegi
{
    private const int MusteriSayisi = 10;
    private const int IslemSayisi = 1000;

    public static async Task RunAsync()
    {
        var hesap = new BankaHesabi(100_000);

        // 10 müşteri aynı anda işlem yapıyor.
        // Her müşteri 1000 kez 10 TL yatırıyor ve 1000 kez 5 TL çekiyor.
        // Beklenen bakiye: 100.000 + 100.000 - 50.000 = 150.000 TL
        var musteriler = Enumerable.Range(1, MusteriSayisi).Select(_ => Task.Run(() =>
        {
            for (int i = 0; i < IslemSayisi; i++)
            {
                hesap.ParaYatir(10);
                hesap.ParaCek(5);
            }
        }));

        // Bütün müşterilerin işi bitene kadar bekle
        await Task.WhenAll(musteriler);

        const decimal beklenen = 150_000;
        var durum = hesap.Bakiye == beklenen ? "✅ doğru" : "❌ bakiye bozuldu!";
        Console.WriteLine($"Son bakiye: {hesap.Bakiye:N0} TL (beklenen: {beklenen:N0} TL)  {durum}");
    }
}