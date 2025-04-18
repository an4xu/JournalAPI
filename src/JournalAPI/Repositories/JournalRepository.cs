using JournalAPI.Services.Journal.Interfaces;

namespace JournalAPI.Repositories;

public class JournalRepository : IJournalRepository
{
    private readonly ILogger<JournalRepository> logger;

    public JournalRepository(ILogger<JournalRepository> logger)
    {
        this.logger = logger;
    }

    public void SaveDiff<TObj, TUpdate>(TObj obj, TUpdate update)
        where TObj : IUniqueObject
        where TUpdate : IUpdate<TObj>
    {
        if (update.HasDiff(obj, out var diff))
        { 
            logger.LogInformation(
                "{type} ({Id}) has been updated. Diff: {Diff}",
                typeof(TObj).Name,
                obj.GetUniqueKey(),
                diff);
        }
        else
        {
            logger.LogInformation(
                "{type} ({Id}) has not been updated.",
                typeof(TObj).Name,
                obj.GetUniqueKey());
        }
    }
}
