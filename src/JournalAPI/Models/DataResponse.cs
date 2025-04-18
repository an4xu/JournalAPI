namespace JournalAPI.Models;

public record DataResponse<T>(bool Success, T Data) : BaseResponse(Success);
