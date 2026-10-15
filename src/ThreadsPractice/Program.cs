using System.Text;
using ThreadsPractice.Examples;

Console.OutputEncoding = Encoding.UTF8;

var examples = new (string Title, Func<Task> Run)[]
{
    ("Thread Temelleri (garson örneği)",          ThreadBasics.RunAsync),
    ("Thread vs Task",                            ThreadVsTask.RunAsync),
    ("async / await (iş bekler, işçi beklemez)",  AsyncAwait.RunAsync),
    ("Multi-thread (tek garson vs çok garson)",   MultiThreading.RunAsync),
    ("Race Condition ve lock (kasa örneği)",      RaceCondition.RunAsync),
};

// Komut satırından numara verilirse direkt o örneği çalıştır: dotnet run -- 3
if (args.Length > 0 && int.TryParse(args[0], out var direct) && direct >= 1 && direct <= examples.Length)
{
    await RunExample(direct);
    return;
}

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== THREADS PRATİK ===");
    for (int i = 0; i < examples.Length; i++)
        Console.WriteLine($"{i + 1}) {examples[i].Title}");
    Console.WriteLine("0) Çıkış");
    Console.Write("Seçim: ");

    var input = Console.ReadLine();
    if (input is null || input == "0") break;

    if (int.TryParse(input, out var choice) && choice >= 1 && choice <= examples.Length)
        await RunExample(choice);
    else
        Console.WriteLine("Geçersiz seçim.");
}

async Task RunExample(int number)
{
    var example = examples[number - 1];
    Console.WriteLine();
    Console.WriteLine($"--- {number}) {example.Title} ---");
    await example.Run();
}
