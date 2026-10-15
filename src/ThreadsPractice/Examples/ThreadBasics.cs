namespace ThreadsPractice.Examples;

/// <summary>
/// KONU: Thread nedir?
/// Thread = işi yapan çalışan (garson).
/// Main metodu da bir thread üzerinde çalışır (Main thread).
/// </summary>
public static class ThreadBasics
{
    public static Task RunAsync()
    {
        Console.WriteLine($"Main thread Id: {Environment.CurrentManagedThreadId}");

        // Yeni bir garson (thread) oluşturuyoruz ve ona bir iş veriyoruz
        var garson1 = new Thread(() => SiparisAl("Masa 1"));
        var garson2 = new Thread(() => SiparisAl("Masa 2"));

        // Start() demeden thread çalışmaz
        garson1.Start();
        garson2.Start();

        // Join() = "bu garson işini bitirene kadar bekle"
        // Join olmasa Main thread garsonları beklemeden devam ederdi
        garson1.Join();
        garson2.Join();

        Console.WriteLine("İki garson da işini bitirdi.");
        Console.WriteLine("NOT: Çıktıların sırası her çalıştırmada değişebilir, çünkü thread'ler aynı anda çalışır.");

        return Task.CompletedTask;
    }

    private static void SiparisAl(string masa)
    {
        // Her thread'in kendi Id'si vardır: farklı garsonlar olduğunu buradan görürüz
        Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] {masa} siparişi alınıyor...");
        Thread.Sleep(500); // iş 0.5 saniye sürüyor
        Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] {masa} siparişi tamam.");
    }
}
