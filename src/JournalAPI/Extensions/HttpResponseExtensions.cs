namespace JournalAPI.Extensions;

public static class HttpResponseExtensions
{
    public static bool IsSuccessful(this HttpResponse response)
    {
        return response.StatusCode >= 200 && response.StatusCode < 300;
    }
}
