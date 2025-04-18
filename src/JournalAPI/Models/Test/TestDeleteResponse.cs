namespace JournalAPI.Models.Test;

public record TestDeleteResponse(TestDataModel Data) : DataResponse<TestDataModel>(true, Data);
