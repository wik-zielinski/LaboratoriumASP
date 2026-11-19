using System.Collections.Concurrent;

namespace LabApp.Middleware;

public class RequestStats                 
{
    private long _total;
    // odczyt stanu licznika
    public long Total => Interlocked.Read(ref _total);
    // słownik przechowywujący ilość wywołań dla każdej ścieżki
    public ConcurrentDictionary<string, long> PathCounts { get; } = new();
    // inkrementacja licznika
    public long Hit(string path)
    {
        PathCounts.AddOrUpdate(path, 1, (_, currentCount) => currentCount + 1);
        return Interlocked.Increment(ref _total);
    }
    
    public IReadOnlyList<KeyValuePair<string, long>> GetTopPaths(int count = 5) => 
        PathCounts.OrderByDescending(kv => kv.Value).Take(count).ToList();
}