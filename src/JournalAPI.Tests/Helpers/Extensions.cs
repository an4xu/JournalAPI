using JournalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace JournalAPI.Tests.Helpers;

internal static class Extensions
{
    public static void AssertOk<T>(this ActionResult response, out T data)
    {
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<OkObjectResult>());

        var okObjectResult = response as OkObjectResult;
        Assert.That(okObjectResult, Is.Not.Null);
        Assert.That(okObjectResult.Value, Is.Not.Null);

        var dataResponse = okObjectResult.Value as DataResponse<T>;
        Assert.That(dataResponse, Is.Not.Null);
        Assert.That(dataResponse.Data, Is.Not.Null);
        Assert.That(dataResponse.Success, Is.True);

        data = dataResponse.Data;
    }

    public static void AssertNotFound(this ActionResult response)
    {
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<NotFoundObjectResult>());

        var okObjectResult = response as NotFoundObjectResult;
        Assert.That(okObjectResult, Is.Not.Null);
        Assert.That(okObjectResult.Value, Is.Not.Null);

        var baseResponse = okObjectResult.Value as BaseResponse;
        Assert.That(baseResponse, Is.Not.Null);
        Assert.That(baseResponse.Success, Is.False);
    }

    public static void AssertCreatedAtAction<T>(this ActionResult response, out T data)
    { 
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.TypeOf<CreatedAtActionResult>());

        var createdAtActionResult = response as CreatedAtActionResult;
        Assert.That(createdAtActionResult, Is.Not.Null);
        Assert.That(createdAtActionResult.Value, Is.Not.Null);

        var dataResponse = createdAtActionResult.Value as DataResponse<T>;
        Assert.That(dataResponse, Is.Not.Null);
        Assert.That(dataResponse.Data, Is.Not.Null);
        Assert.That(dataResponse.Success, Is.True);

        data = dataResponse.Data;
    }
}
