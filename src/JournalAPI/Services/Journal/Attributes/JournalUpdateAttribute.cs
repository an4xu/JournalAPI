using JournalAPI.Extensions;
using JournalAPI.Services.Journal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace JournalAPI.Services.Journal.Attributes;

public class JournalUpdateAttribute<TObj, TUpdate> : JournalAttribute<TObj>
    where TObj : IUniqueObject
    where TUpdate : IUpdate<TObj>
{
    private readonly string idPropertyName;
    private readonly string updatePropertyName;

    private string? traceId;
    private TObj? tracedObject;
    private TUpdate? update;

    public JournalUpdateAttribute(string idPropertyName, string updatePropertyName)
    {
        this.idPropertyName = idPropertyName;
        this.updatePropertyName = updatePropertyName;
    }

    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!HasTraceId(context.HttpContext, out traceId))
        {
            context.Result = new BadRequestObjectResult("Missing x-journal header");
            return Task.CompletedTask;
        }

        if (!HasUpdateArgument(context, out update) || !HasIdArgument(context, out var id))
        {
            context.Result = new BadRequestObjectResult("Missing update or id argument");
            return Task.CompletedTask;
        }

        var journalService = GetJournalService(context.HttpContext);

        if (!journalService.Remove<TObj>(id, traceId, out tracedObject))
        {
            context.Result = new NotFoundObjectResult("Journal entry not found");
            return Task.CompletedTask;
        }

        return base.OnActionExecutionAsync(context, next);
    }

    public override void OnResultExecuted(ResultExecutedContext context)
    {
        var journalService = GetJournalService(context.HttpContext);

        if(tracedObject is null || update is null || string.IsNullOrWhiteSpace(traceId))
        {
            return;
        }

        if (!context.HttpContext.Response.IsSuccessful())
        {
            journalService.Set(traceId, tracedObject!);
        }
        else
        {
            var journalRepository = GetJournalRepository(context.HttpContext);
            journalRepository.SaveDiff(tracedObject, update);
        }
    }

    private bool HasUpdateArgument(ActionExecutingContext context, [NotNullWhen(true)] out TUpdate? update)
    {
        if (context.ActionArguments.TryGetValue(updatePropertyName, out var updateObject)
            && updateObject is TUpdate updateValue)
        {
            update = updateValue;
            return true;
        }

        update = default;
        return false;
    }

    private bool HasIdArgument(ActionExecutingContext context, [NotNullWhen(true)] out string id)
    {
        if (context.ActionArguments.TryGetValue(idPropertyName, out var idObject)
            && idObject is not null)
        {
            id = idObject.ToString() ?? string.Empty;
            return !string.IsNullOrWhiteSpace(id);
        }

        id = string.Empty;
        return false;
    }
}
