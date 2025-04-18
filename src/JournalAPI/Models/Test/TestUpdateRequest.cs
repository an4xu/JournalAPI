using JournalAPI.Services.Journal.Interfaces;

namespace JournalAPI.Models.Test;

public record TestUpdateRequest(string Name, DateTime DateTime) : IUpdate<TestDataModel>
{
    public bool HasDiff(TestDataModel obj, out string diff)
    {
        var differences = new List<string>(2);

        if (Name != obj.Name)
        {
            differences.Add($"Name: {obj.Name} -> {Name}");
        }

        if (DateTime != obj.DateTime)
        {
            differences.Add($"DateTime: {obj.DateTime} -> {DateTime}");
        }

        if (differences.Count != 0)
        {
            diff = string.Join("; ", differences);
            return true;
        }

        diff = string.Empty;
        return false;
    }
}
