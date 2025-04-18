using JournalAPI.Models.Test;

namespace JournalAPI.Repositories;

public interface ITestDataRepository
{
    TestDataModel Create(TestCreateRequest create);
    TestDataModel? Delete(int id);
    TestDataModel? Get(int id);
    TestDataModel[] GetAll();
    TestDataModel? Update(int id, TestUpdateRequest update);
}