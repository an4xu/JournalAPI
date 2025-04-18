using JournalAPI.Services.Journal.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace JournalAPI.Services.Journal;

public class JournalService : IJournalService
{
    private readonly IMemoryCache memoryCache;
    private readonly TimeSpan expirationTime;

    public JournalService(IMemoryCache memoryCache, IOptions<JournalOptions> options)
    {
        this.memoryCache = memoryCache;
        expirationTime = TimeSpan.FromSeconds(options.Value.ExpirationTimeSeconds);
    }

    public string Trace<TObj>(TObj value) 
        where TObj : IUniqueObject
    {
        var traceId = GetNewTraceId();
        var key = GetKey(value, traceId);
        memoryCache.Set(key, value, expirationTime);
        return traceId;
    }

    public bool Remove<TObj>(string id, string traceId, [NotNullWhen(true)] out TObj value)
        where TObj : IUniqueObject
    {
        var key = GetKey<TObj>(id, traceId);
        if (memoryCache.TryGetValue(key, out value!) && value is not null)
        {
            memoryCache.Remove(key);
            return true;
        }

        return false;
    }

    public void Set<TObj>(string traceId, TObj value)
        where TObj : IUniqueObject
    {
        var key = GetKey(value, traceId);
        memoryCache.Set(key, value, expirationTime);
    }

    private static string GetNewTraceId()
    {
        return Guid.NewGuid().ToString();
    }

    private static string GetKey<TObj>(TObj obj, string traceId) where TObj : IUniqueObject
    {
        return GetKey<TObj>(obj.GetUniqueKey(), traceId);
    }

    private static string GetKey<TObj>(string id, string traceId) where TObj : IUniqueObject
    {
        return $"{typeof(TObj).Name}_{id}_{traceId}";
    }
}
