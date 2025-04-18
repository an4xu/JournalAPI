using JournalAPI.Repositories;
using JournalAPI.Services.Journal.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JournalAPI.Services.Journal.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public abstract class JournalAttribute : ActionFilterAttribute
{
    public const string TraceIdHeader = "x-journal";
}


public abstract class JournalAttribute<TObj> : JournalAttribute
    where TObj : IUniqueObject
{ 
    protected IJournalService GetJournalService(HttpContext context)
    {
        var result = context.RequestServices
            .GetRequiredService<IJournalService>();

        return result;
    }

    protected IJournalRepository GetJournalRepository(HttpContext context)
    { 
        var result = context.RequestServices
            .GetRequiredService<IJournalRepository>();

        return result;
    }

    protected void AddTraceIdHeader(HttpContext context, string traceId)
    { 
        context.Response.Headers.Append(TraceIdHeader, traceId);
    }

    protected bool HasTraceId(HttpContext context, out string traceId)
    {
        var header = context.Request.Headers[TraceIdHeader];
        traceId = header.ToString();
        return !string.IsNullOrWhiteSpace(traceId);
    }
}
