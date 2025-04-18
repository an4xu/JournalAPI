namespace JournalAPI.Services.Journal.Interfaces;

public interface IUpdate<T>
{
    public bool HasDiff(T obj, out string diff);
}