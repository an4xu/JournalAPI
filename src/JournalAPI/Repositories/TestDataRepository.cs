using JournalAPI.Models.Test;

namespace JournalAPI.Repositories;

public class TestDataRepository : ITestDataRepository
{
    private static readonly List<TestDataModel> data =
    [
        new TestDataModel(1, "Test1", DateTime.UtcNow),
        new TestDataModel(2, "Test2", DateTime.Now),
    ];

    public TestDataModel[] GetAll()
    {
        return [.. data];
    }

    public TestDataModel? Get(int id)
    {
        return data.FirstOrDefault(x => x.Id == id);
    }

    public TestDataModel Create(TestCreateRequest create)
    {
        var newId = data.Max(x => x.Id) + 1;
        var newItem = new TestDataModel(newId, create.Name, create.DateTime);
        data.Add(newItem);
        return newItem;
    }

    public TestDataModel? Update(int id, TestUpdateRequest update)
    {
        var existing = data.FirstOrDefault(x => x.Id == id);
        if (existing is null)
        {
            return null;
        }
        existing = existing with
        {
            Name = update.Name,
            DateTime = update.DateTime,
        };
        return existing;
    }

    public TestDataModel? Delete(int id)
    {
        var existing = data.FirstOrDefault(x => x.Id == id);

        return existing is not null && data.Remove(existing)
            ? existing 
            : null;
    }
}
