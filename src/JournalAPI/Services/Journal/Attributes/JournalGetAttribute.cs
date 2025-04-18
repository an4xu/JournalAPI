using JournalAPI.Models;
using JournalAPI.Services.Journal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace JournalAPI.Services.Journal.Attributes;

public class JournalGetAttribute<TObj> : JournalAttribute<TObj>
    where TObj : IUniqueObject
{
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        base.OnResultExecuted(context);
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (IsDataResponse(context.Result, out var dataResponse))
        {
            var journalService = GetJournalService(context.HttpContext);

            var traceId = journalService.Trace(dataResponse.Data);
            AddTraceIdHeader(context.HttpContext, traceId);
        }

        base.OnResultExecuting(context);
    }

    private static bool IsDataResponse(IActionResult result, [NotNullWhen(true)] out DataResponse<TObj>? response)
    {
        if (result is ObjectResult objectResult && objectResult.Value is DataResponse<TObj> dataResponse)
        {
            response = dataResponse;
            return true;
        }

        response = null;
        return false;
    }
}
