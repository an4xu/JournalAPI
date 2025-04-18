namespace JournalAPI.Services.Journal;

public class JournalOptions
{
    public int ExpirationTimeSeconds { get; set; } = (int)TimeSpan.FromHours(1).TotalSeconds;
}
