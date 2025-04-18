namespace JournalAPI.Models.Test;

public record TestCreateResponse(TestDataModel Data) : DataResponse<TestDataModel>(true, Data);
