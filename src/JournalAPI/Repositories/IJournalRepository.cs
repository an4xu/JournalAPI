using JournalAPI.Services.Journal.Interfaces;

namespace JournalAPI.Repositories;

public interface IJournalRepository
{
    void SaveDiff<TObj, TUpdate>(TObj obj, TUpdate update)
        where TObj : IUniqueObject
        where TUpdate : IUpdate<TObj>;
}
