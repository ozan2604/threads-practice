# 🧵 Threads Practice (.NET 8)

C# ile **Thread, Task, async/await, Multi-threading ve Race Condition** konularını restoran benzetmesiyle anlatan pratik projesi.

> 🍽️ **Benzetme:** Program = restoran, Thread = garson, Task = garsona verilen görev, ThreadPool = hazır bekleyen garson ekibi.

## 🚀 Çalıştırma

```bash
dotnet run --project src/ThreadsPractice        # menüden seç
dotnet run --project src/ThreadsPractice -- 5   # direkt 5. örneği çalıştır
```

## 📚 Konular

| # | Konu | Dosya | Ne gösteriyor? |
|---|------|-------|----------------|
| 1 | Thread Temelleri | [ThreadBasics.cs](src/ThreadsPractice/Examples/ThreadBasics.cs) | `new Thread`, `Start`, `Join`, thread Id'leri |
| 2 | Thread vs Task | [ThreadVsTask.cs](src/ThreadsPractice/Examples/ThreadVsTask.cs) | Değer döndürme, hata yakalama, ThreadPool'un thread'leri tekrar kullanması |
| 3 | async / await | [AsyncAwait.cs](src/ThreadsPractice/Examples/AsyncAwait.cs) | Thread'i kilitlemeden bekleme, `Task.WhenAll`, `.Result` tehlikesi |
| 4 | Multi-thread | [MultiThreading.cs](src/ThreadsPractice/Examples/MultiThreading.cs) | Tek thread vs çok thread süresi, `Parallel.For` |
| 5 | Race Condition | [RaceCondition.cs](src/ThreadsPractice/Examples/RaceCondition.cs) | Kaybolan para, `lock` ve `Interlocked` ile çözüm |

## 📊 Örnek Çıktılar (8 çekirdekli makine)

```
Sırayla await : 3014 ms
Task.WhenAll  : 1013 ms

Tek thread    : 3013 ms
Multi-thread  : 1012 ms

Kilitsiz (kasa++)   :      220,125 TL  ❌ 779,875 TL kayboldu!
lock ile            :    1,000,000 TL  ✅ doğru
Interlocked ile     :    1,000,000 TL  ✅ doğru
```

## 🧠 Kısa Özet

| Kavram | Restoranda | Tek cümle |
|---|---|---|
| **Thread** | Garson | İşi yapan çalışan |
| **Task** | Garsona verilen görev | ThreadPool kullanır, değer döner, hata yakalanır |
| **async/await** | Yemeği beklerken başka masaya bakmak | **İş bekler, işçi beklemez** |
| **Multi-thread** | Birden fazla garson | İşler aynı anda yapılır |
| **lock** | Kasanın kapısı | Aynı anda tek thread girer |

## 🎯 Mülakat Soruları

1. **Thread ile Task farkı nedir?**
   Thread bir çalışandır, Task bir görevdir. Task, ThreadPool'daki hazır thread'leri kullandığı için daha verimlidir; değer döndürebilir, hatası `await` ile yakalanabilir.
2. **async/await yeni thread açar mı?**
   Hayır. Metot task bitene kadar bekler ama thread'i kilitlemez; thread o sırada başka işlere bakar.
3. **`.Result` / `.Wait()` neden tehlikelidir?**
   Thread'i kilitler; UI ve eski ASP.NET uygulamalarında deadlock'a yol açabilir.
4. **Race condition nedir, nasıl önlenir?**
   Birden fazla thread aynı veriyi aynı anda değiştirince sonucun bozulmasıdır. `lock`, `Interlocked` veya thread-safe koleksiyonlarla önlenir.
5. **Multi-thread ile async/await farkı nedir?**
   Multi-thread daha çok çalışanla işi bölüştürür (CPU-bound). async/await aynı çalışanın boşa beklemesini engeller (I/O-bound).

## ✍️ Kendin Dene (Alıştırmalar)

- [ ] **RaceCondition:** `GarsonSayisi` değerini 1 yap. Kilitsiz versiyonda hâlâ para kayboluyor mu? Neden?
- [ ] **AsyncAwait:** `MutfakAsync` içindeki `Task.Delay`'i `Thread.Sleep` ile değiştir. Ne fark etti?
- [ ] **MultiThreading:** `Parallel.For` ile 8 yerine 32 hesap yap. Süre 4 katına mı çıktı? Çekirdek sayınla ilişkisini düşün.
- [ ] **Yeni örnek:** Bir `BankaHesabi` sınıfı yaz: `ParaYatir` ve `ParaCek` metotları olsun. 10 thread aynı anda işlem yapsın, bakiye bozulmasın.
- [ ] **Yeni örnek:** 5 farklı "API çağrısını" (`Task.Delay` ile) önce sırayla, sonra `Task.WhenAll` ile çağır ve süreleri karşılaştır.

## 🔜 Sıradaki Konular

- Deadlock
- SemaphoreSlim (aynı anda en fazla N işlem)
- CancellationToken (işi iptal etmek)
- Concurrent koleksiyonlar (`ConcurrentDictionary`, `ConcurrentQueue`)
