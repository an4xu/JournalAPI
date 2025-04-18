namespace JournalAPI.Models.Test;

public record TestUpdateResponse(TestDataModel Data) : DataResponse<TestDataModel>(true, Data);
