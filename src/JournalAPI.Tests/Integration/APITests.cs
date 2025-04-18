using JournalAPI.Models;
using JournalAPI.Models.Test;
using JournalAPI.Repositories;
using JournalAPI.Services.Journal.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Moq.AutoMock;
using System.Net.Http.Json;
using System.Security.Authentication;

namespace JournalAPI.Tests.Integration;

public class APITests
{
    private WebApplicationFactory<Program> factory;
    private AutoMocker mocker;

    [SetUp]
    public void Setup()
    {
        mocker = new AutoMocker();
        factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IJournalRepository>();
                    services.AddScoped(services => mocker.Get<IJournalRepository>());

                });
            });
    }

    [TearDown]
    public void TearDown()
    {
        factory.Dispose();
    }

    [Test]
    public async Task GetTests_ReturnsSuccess()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/tests");

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [Test]
    public async Task GetTest_Return_TraceId()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/tests/1");
        var traceId = response.Headers.GetValues(JournalAttribute.TraceIdHeader).FirstOrDefault();

        Assert.That(traceId, Is.Not.Null);
        Assert.That(traceId, Is.Not.Empty);
        Assert.That(traceId, Is.Not.EqualTo(Guid.Empty.ToString()));
    }

    [Test]
    public async Task GetTest_Return404_IfTestNotFound()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/tests/9999");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Update_Return400_IfTraceIdNotProvided()
    {
        var client = factory.CreateClient();
        var request = new TestUpdateRequest("NewName", DateTime.UtcNow);

        var response = await client.PutAsJsonAsync("/tests/1", request);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Update_Return404_IfTraceIdNotFound()
    {
        var client = factory.CreateClient();
        var request = new TestUpdateRequest("NewName", DateTime.UtcNow);

        client.DefaultRequestHeaders.Add(JournalAttribute.TraceIdHeader, Guid.NewGuid().ToString());
        var response = await client.PutAsJsonAsync("/tests/1", request);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }

    [Test]
    public async Task Update_Return200_IfTraceIdFound()
    {
        var client = factory.CreateClient();

        var getRequest = await client.GetAsync("/tests/1");
        var traceId = getRequest.Headers.GetValues(JournalAttribute.TraceIdHeader).FirstOrDefault();

        var request = new TestUpdateRequest("NewName", DateTime.UtcNow);
        client.DefaultRequestHeaders.Add(JournalAttribute.TraceIdHeader, traceId);

        var response = await client.PutAsJsonAsync("/tests/1", request);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
    }

    [Test]
    public async Task Update_AddDifferenceToJournalRepository()
    {
        var client = factory.CreateClient();
        var updateRequest = new TestUpdateRequest("NewName", DateTime.UtcNow);

        var getResponse = await client.GetAsync("/tests/1");
        var getResult = await getResponse.Content.ReadFromJsonAsync<DataResponse<TestDataModel>>();
        var traceId = getResponse.Headers.GetValues(JournalAttribute.TraceIdHeader).FirstOrDefault();


        client.DefaultRequestHeaders.Add(JournalAttribute.TraceIdHeader, traceId);
        var putResponse = await client.PutAsJsonAsync("/tests/1", updateRequest);
        var putResult = await putResponse.Content.ReadFromJsonAsync<DataResponse<TestDataModel>>();


        var journalRepository = mocker.Get<IJournalRepository>();
        Assert.That(getResult, Is.Not.Null);
        Assert.That(getResult.Data, Is.Not.Null);
        mocker.GetMock<IJournalRepository>()
            .Verify(x => x.SaveDiff(
                It.Is<TestDataModel>(model => model.Id == getResult.Data.Id && model.Name == getResult.Data.Name && model.DateTime == getResult.Data.DateTime),
                It.Is<TestUpdateRequest>(x => x.Name == updateRequest.Name && x.DateTime == updateRequest.DateTime)), Times.Once);

        Assert.That(putResult, Is.Not.Null);
        Assert.That(putResult.Data, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(putResult.Data.Name, Is.EqualTo(updateRequest.Name));
            Assert.That(putResult.Data.DateTime, Is.EqualTo(updateRequest.DateTime));
        });
    }
}