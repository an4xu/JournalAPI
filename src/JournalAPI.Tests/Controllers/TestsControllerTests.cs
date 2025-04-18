using JournalAPI.Controllers;
using JournalAPI.Models.Test;
using JournalAPI.Repositories;
using JournalAPI.Tests.Helpers;
using Moq;
using Moq.AutoMock;

namespace JournalAPI.Tests.Controllers;

internal class TestsControllerTests
{
    private AutoMocker mocker;

    [SetUp]
    public void Setup()
    {
        mocker = new AutoMocker();
    }

    [Test]
    public void GetAll_Return_TestGetAllResponse()
    {
        // Arrange
        var controller = mocker.CreateInstance<TestsController>();
        var expectedTests = new List<TestDataModel>
        {
            new (1, "Test1", DateTime.UtcNow),
            new (2, "Test2", DateTime.Now),
        };

        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.GetAll())
            .Returns(expectedTests.ToArray());

        // Act
        var response = controller.GetAll();

        // Assert
        response.AssertOk(out TestDataModel[] data);
        Assert.That(data, Is.EqualTo(expectedTests));
    }

    [Test]
    public void Get_Return_TestGetResponse_If_TestFound()
    {
        // Arrange
        var controller = mocker.CreateInstance<TestsController>();
        var expectedTest = new TestDataModel(1, "Test1", DateTime.UtcNow);

        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.Get(1))
            .Returns(expectedTest);

        // Act
        var response = controller.Get(1);

        // Assert
        response.AssertOk(out TestDataModel data);
        Assert.That(data, Is.EqualTo(expectedTest));
    }

    [Test]
    public void Get_Return_404_If_TestNotFound()
    {
        // Arrange
        var controller = mocker.CreateInstance<TestsController>();
        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.Get(1))
            .Returns((TestDataModel?)null);

        // Act
        var response = controller.Get(1);

        // Assert
        response.AssertNotFound();
    }

    [Test]
    public void Update_Return_TestUpdateResponse_If_TestFound()
    {
        // Arrange
        var controller = mocker.CreateInstance<TestsController>();
        var updateRequest = new TestUpdateRequest("UpdatedName", DateTime.UtcNow);
        var expectedTest = new TestDataModel(1, updateRequest.Name, updateRequest.DateTime);

        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.Update(1, updateRequest))
            .Returns(expectedTest);

        // Act
        var response = controller.Update(1, updateRequest);

        // Assert
        response.AssertOk(out TestDataModel data);
        Assert.That(data, Is.EqualTo(expectedTest));
    }

    [Test]
    public void Update_Return_404_If_TestNotFound()
    {
        // Arrange
        var controller = mocker.CreateInstance<TestsController>();
        var updateRequest = new TestUpdateRequest("UpdatedName", DateTime.UtcNow);

        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.Update(1, updateRequest))
            .Returns((TestDataModel?)null);

        // Act
        var result = controller.Update(1, updateRequest);

        // Assert
        result.AssertNotFound();
    }

    [Test]
    public void Create_Return_TestCreateResponse()
    {
        // Arrange  
        var controller = mocker.CreateInstance<TestsController>();
        var createRequest = new TestCreateRequest("NewTest", DateTime.UtcNow);
        var expectedTest = new TestDataModel(1, createRequest.Name, createRequest.DateTime);

        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.Create(It.IsAny<TestCreateRequest>()))
            .Returns(expectedTest);

        // Act  
        var response = controller.Create(createRequest);

        // Assert
        response.AssertCreatedAtAction(out TestDataModel data);
        Assert.That(data, Is.EqualTo(expectedTest));
    }

    [Test]
    public void Delete_Return_TestDeleteResponse_If_TestFound()
    {
        // Arrange
        var controller = mocker.CreateInstance<TestsController>();
        var expectedTest = new TestDataModel(1, "Test1", DateTime.UtcNow);

        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.Delete(1))
            .Returns(expectedTest);

        // Act
        var response = controller.Delete(1);

        // Assert
        response.AssertOk(out TestDataModel data);
        Assert.That(data, Is.EqualTo(expectedTest));
    }

    [Test]
    public void Delete_Return_404_If_TestNotFound()
    {
        // Arrange
        var controller = mocker.CreateInstance<TestsController>();
        mocker.GetMock<ITestDataRepository>()
            .Setup(repo => repo.Delete(1))
            .Returns((TestDataModel?)null);

        // Act
        var response = controller.Delete(1);

        // Assert
        response.AssertNotFound();
    }
}
