using JournalAPI.Services.Journal.Interfaces;

namespace JournalAPI.Services.Journal;

public interface IJournalService
{
    string Trace<TObj>(TObj value)
        where TObj : IUniqueObject;

    bool Remove<TObj>(string id, string traceId, out TObj value)
        where TObj : IUniqueObject;

    void Set<TObj>(string traceId, TObj value)
        where TObj : IUniqueObject;
}
