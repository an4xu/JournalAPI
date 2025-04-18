namespace JournalAPI.Models.Test;

public record TestGetResponse(TestDataModel Data) : DataResponse<TestDataModel>(true, Data);
