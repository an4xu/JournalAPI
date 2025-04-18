namespace JournalAPI.Models.Test;

public record TestGetAllResponse(TestDataModel[] Data) : DataResponse<TestDataModel[]>(true, Data);
