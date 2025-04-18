using JournalAPI.Extensions;
using Microsoft.AspNetCore.Http;

namespace JournalAPI.Tests.Extensions;

internal class HttpResponseExtensionsTests
{
    public static object[] SuccessfulStatusCodes =
    [
        new object[] { 100, false},
        new object[] { 300, false},
        new object[] { 400, false},
        new object[] { 401, false},
        new object[] { 403, false},
        new object[] { 404, false},
        new object[] { 405, false},
        new object[] { 406, false},
        new object[] { 409, false},
        new object[] { 500, false},
        new object[] { 200, true },
        new object[] { 201, true },
        new object[] { 202, true },
        new object[] { 203, true },
        new object[] { 204, true },
        new object[] { 205, true },
        new object[] { 206, true },
        new object[] { 207, true },
        new object[] { 208, true },
        new object[] { 226, true }
    ];


    [Test, Combinatorial]
    [TestCaseSource(nameof(SuccessfulStatusCodes))]
    public void IsSuccessful_Returns_CorrectResult(int statusCode, bool expectedResult)
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.StatusCode = statusCode;

        // Act
        var result = httpContext.Response.IsSuccessful();

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}
